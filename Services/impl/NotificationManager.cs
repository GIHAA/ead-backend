using System.Collections.Concurrent;

namespace TechFixBackend.Services
{
    public class NotificationManager
    {
        // A thread-safe dictionary to manage user connections
        private readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();

        

        // Add a new connection
        public void AddConnection(string userId, string connectionId)
        {
            _connections.TryAdd(connectionId, userId);
        }

        // Remove a connection when a user disconnects
        public void RemoveConnection(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
        }

        // Get user by connection ID
        public string GetUserByConnectionId(string connectionId)
        {
            _connections.TryGetValue(connectionId, out var userId);
            return userId;
        }

        // Get all connections for a user
        public IEnumerable<string> GetConnections(string userId)
        {
            return _connections.Where(kvp => kvp.Value == userId).Select(kvp => kvp.Key);
        }
    }
}
