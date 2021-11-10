using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.BackendGame.Handlers
{
    public class UserJoinGameHandler: INotificationHandler<UserJoinGame>
    {
        private readonly GameContext _db;
        private readonly IHubContext<GamesHub> _hubContext;

        public UserJoinGameHandler(GameContext db, IHubContext<GamesHub> hubContext)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _hubContext = hubContext;
        }
        public async Task Handle(UserJoinGame domainEvent, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("GameRoomUpdated", domainEvent.Notification, cancellationToken);
        }
    }
}