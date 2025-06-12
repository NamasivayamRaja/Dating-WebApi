namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUser = [];

        public Task<bool> UserConnected(string userName, string connectionId)
        {
            var isOnline = false;
            lock (OnlineUser)
            {
                if (OnlineUser.ContainsKey(userName))
                {
                    OnlineUser[userName].Add(connectionId);
                }
                else
                {
                    OnlineUser.Add(userName, [connectionId]);
                    isOnline = true;
                }
            }
            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string userName, string connectionId)
        {
            var isOffline = false;
            lock (OnlineUser)
            {
                if (!OnlineUser.ContainsKey(userName)) return Task.FromResult(isOffline);

                OnlineUser[(userName)].Remove(connectionId);

                if (OnlineUser[userName].Count == 0)
                {
                    OnlineUser.Remove(userName);
                    isOffline = true;
                }
            }
            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUser()
        {
            string[] onlineUsers;
            lock (OnlineUser)
            {
                onlineUsers = OnlineUser.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        }

        public static Task<List<string>> GetConnectionsForUser(string userName)
        {
            List<string> connectionIds;

            if(OnlineUser.TryGetValue(userName, out var connections))
            {
                lock(connections)
                {
                    connectionIds = [.. connections];
                }
            }
            else
            {
                connectionIds = [];
            }

            return Task.FromResult(connectionIds);
        }
    }
}
