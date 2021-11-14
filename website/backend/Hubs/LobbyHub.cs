using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Pipelines;
using Domain.Authentication;

namespace backend.Hubs
{
    public class GamesHub: Hub
    {
        private readonly IConnectionIdsStorage _connectionIdsStorage;
        private static readonly ConcurrentDictionary<string, User> Users = new();

        public GamesHub(IConnectionIdsStorage connectionIdsStorage)
        {
            _connectionIdsStorage = connectionIdsStorage;
        }
        
        public override Task OnConnectedAsync() {
            
            _connectionIdsStorage.SetConnectionId(Guid.Parse(Context.UserIdentifier), Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        
        public override Task OnDisconnectedAsync(Exception exception) {
            _connectionIdsStorage.RemoveConnections(Guid.Parse(Context.UserIdentifier));
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(Game game)
        {
            await Clients.All.SendAsync("GameCreated", game);
        }
        
        public async Task Test(Game game)
        {
            await Clients.All.SendAsync("GameCreated", game);
        }
    }
}
