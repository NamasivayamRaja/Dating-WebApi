using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub(PresenceTracker presenceTracker, ILogger<PresenceHub> logger) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            try
            {
                if (Context.User == null) throw new Exception("Cannot get current user claim");

                var isOnline = await presenceTracker.UserConnected(Context.User.GetUserName(), Context.ConnectionId);
                // notify other clients
                if (isOnline)
                {
                    await Clients.Others.SendAsync("UserIsOnline", Context.User?.GetUserName());
                }

                var currentUsers = await presenceTracker.GetOnlineUser();

                await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
            }
            catch (Exception ex)
            {
                logger.LogError($"OnConnectedAsync: {ex.Message}");
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User == null) throw new Exception("Cannot get user claim");

            var isOffline = await presenceTracker.UserDisconnected(Context.User.GetUserName(), Context.ConnectionId);

            if (isOffline) 
                await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUserName());
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}
