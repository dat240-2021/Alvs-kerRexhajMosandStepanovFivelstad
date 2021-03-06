using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Events;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Lobby.Handlers
{
    public class GameCreatedHandler : INotificationHandler<GameCreated>
    {
        private readonly GameContext _db;
        private readonly IHubContext<LobbyHub> _hubContext;

        public GameCreatedHandler(GameContext db, IHubContext<LobbyHub> hubContext)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _hubContext = hubContext;
        }

        public async Task Handle(GameCreated domainEvent, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("GameCreated", (domainEvent), cancellationToken);
        }
    }
}