using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace backend.Hubs
{
    public class ConnectionIdsStorage: IConnectionIdsStorage
    {
        public ConcurrentDictionary<Guid, List<string>> ConnectionIds = new ();


        public List<string> GetConnectionIds(Guid id)
        {
            return ConnectionIds.TryGetValue(id, out var connectionIds) ? connectionIds : new List<string>();
        }
        
        public void SetConnectionId(Guid id, string connectionId)
        {
            if (!ConnectionIds.ContainsKey(id))
            {
                ConnectionIds.TryAdd(id, new List<string>());
            }   
            
            if (ConnectionIds.TryGetValue(id, out var connectionIds))
            {
                connectionIds.Add(connectionId);
            }
        }

        public void RemoveConnections(Guid id)
        {
            ConnectionIds.TryRemove(id, out _);
        }
    }

    public interface IConnectionIdsStorage
    {
        public List<string> GetConnectionIds(Guid id);
        public void SetConnectionId(Guid id, string connectionId);
        
        public void RemoveConnections(Guid id);
    }
}