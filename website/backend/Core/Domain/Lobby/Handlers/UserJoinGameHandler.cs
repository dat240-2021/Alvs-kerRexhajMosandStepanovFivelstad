using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Events;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Lobby.Handlers
{
    public class UserJoinGameHandler : INotificationHandler<UserJoinGame>
    {
        private readonly GameContext _db;
        private readonly IHubContext<LobbyHub> _hubContext;

        public UserJoinGameHandler(GameContext db, IHubContext<LobbyHub> hubContext)
        {
            _db = db ?? throw new System.ArgumentNullException(nameof(db));
            _hubContext = hubContext ?? throw new System.ArgumentNullException(nameof(hubContext));
        }
        public async Task Handle(UserJoinGame domainEvent, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("GameRoomUpdated", domainEvent.Notification, cancellationToken);
        }
    }
}