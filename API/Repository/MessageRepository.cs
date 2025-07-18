﻿using API.Data;
using API.DTOs;
using API.Entities;
using API.Helper;
using API.Repository.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MessageRepository(DataContext dataContext, IMapper mapper) : IMessageRepository
    {
        public void AddGroup(Group group)
        {
            dataContext.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            dataContext.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            dataContext.Messages.Remove(message);
        }

        public async Task<Connection?> GetConnection(string connectionId)
        {
            return await dataContext.Connections.FindAsync(connectionId);
        }

        public async Task<Group?> GetGroupForConnection(string connectionId)
        {
            return await dataContext.Groups
                  .Include(x => x.Connections)
                  .Where(x => x.Connections
                  .Any(c => c.ConnectionId == connectionId))
                  .FirstOrDefaultAsync();
        }

        public async Task<Message?> GetMessage(int id)
        {
            return await dataContext.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = dataContext.Messages.OrderByDescending(x=>x.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.RecipientDeleted == false),
                "Outbox" => query.Where(x => x.Sender.UserName == messageParams.UserName && x.SenderDeleted == false),
                _ => query.Where(x => x.Recipient.UserName == messageParams.UserName && x.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<Group?> GetMessageGroup(string groupName)
        {
            return await dataContext.Groups.Include(x => x.Connections).FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var query = dataContext.Messages
                    .Where(
                            x => x.RecipientUserName == currentUserName && x.RecipientDeleted == false && x.SenderUserName == recipientUserName ||
                            x.SenderUserName == currentUserName && x.SenderDeleted == false && x.RecipientUserName == recipientUserName
                          )
                    .OrderBy(x => x.MessageSent)
                    .AsQueryable();

            var unreadMessage = query.Where(x => x.RecipientUserName == currentUserName && x.DateRead == null).ToList();

            if (unreadMessage.Any())
            {
                unreadMessage.ForEach(x => x.DateRead = DateTime.UtcNow);
            }

            return await query.ProjectTo<MessageDto>(mapper.ConfigurationProvider).ToListAsync();
        }

        public void RemoveConnection(Connection connection)
        {
            dataContext.Connections.Remove(connection);
        }
    }
}
