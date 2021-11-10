using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.BackendGame.Handlers
{
    public class UserLeftGameHandler: INotificationHandler<UserLeftGame>
    {
        private readonly GameContext _db;
        private readonly IHubContext<GamesHub> _hubContext;

        public UserLeftGameHandler(GameContext db, IHubContext<GamesHub> hubContext)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _hubContext = hubContext;
        }
        
        public async Task Handle(UserLeftGame domainEvent, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("GameRoomUpdated", domainEvent.Notification, cancellationToken);
        }
    }
}