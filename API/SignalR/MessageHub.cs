using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Repository;
using API.Repository.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class MessageHub(IMessageRepository messageRepository, IUserRepository<AppUser> userRepository, IMapper mapper, IHubContext<PresenceHub> presenceHub) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                var otherUser = httpContext?.Request.Query["user"];

                if (Context.User == null || string.IsNullOrEmpty(otherUser)) throw new Exception("Cannot Join Group");

                var groupName = GetGroupName(Context.User.GetUserName(), otherUser);

                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                var group = await AddToGroup(groupName);

                await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

                var messages = await messageRepository.GetMessageThread(Context.User.GetUserName(), otherUser!);

                await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"onConnected async {ex}");
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var group = await RemoveFromMessageGroup();
                await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"On Disconnected {ex.Message}");
                throw;
            }
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var userName = Context.User?.GetUserName() ??  throw new Exception("Could not get user");

            if (userName == createMessageDto.RecipientUserName.ToLower())
            {
                throw new HubException("Cannot join group");
            }

            var sender = await userRepository.GetByUserNameAsync(userName);
            var recipient = await userRepository.GetByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null || sender == null 
                || sender.UserName == null || recipient.UserName  == null)
            {
                throw new HubException("Cannot send message at this time");
            }

            var message = new Message
            {
                Content = createMessageDto.Content,
                Recipient = recipient,
                Sender = sender,
                SenderUserName = userName,
                RecipientUserName = createMessageDto.RecipientUserName,
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await messageRepository.GetMessageGroup(groupName);

            if(group != null && group.Connections.Any(x=>x.UserName ==  recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if(connections != null && connections?.Count != null)
                {
                    await presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { userName = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            messageRepository.AddMessage(message);

            if (await messageRepository.SaveAllChangeAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
            }
        }

        private string GetGroupName(string caller, string? other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;

            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var userName = Context.User?.GetUserName() ?? throw new Exception("Can not get user name.");
            var group = await messageRepository.GetMessageGroup(groupName);
            var connection =  new Connection { ConnectionId = Context.ConnectionId, UserName = userName };
            
            if(group == null)
            {
                group =  new Group { Name = groupName };
                messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await messageRepository.SaveAllChangeAsync()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await messageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group?.Connections.FirstOrDefault(x=> x.ConnectionId == Context.ConnectionId);

            if(connection != null && group != null)
            {
                messageRepository.RemoveConnection(connection);
                if (await messageRepository.SaveAllChangeAsync()) return group;
            }

            throw new Exception("Failed to remove group");
        }
    }
}
