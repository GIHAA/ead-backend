
/*
 * File: NotificationManager.cs
 * Project: TechFixBackend
 * Description: This file defines the NotificationManager class, which manages user connections in a thread-safe manner. 
 *              It stores and tracks SignalR connection IDs, allowing the system to send targeted notifications to specific users.
 * 
 * Authors: Cooray N.T.L. it21177996 
 * 
 * Classes:
 * - NotificationManager: Manages SignalR connections for users and groups.
 * 
 * Methods:
 * - AddConnection: Adds a new connection for a user.
 * - RemoveConnection: Removes a connection when a user disconnects.
 * - GetUserByConnectionId: Retrieves a userId based on their connectionId.
 * - GetConnections: Retrieves all connection IDs associated with a userId.
 * 
 */


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
