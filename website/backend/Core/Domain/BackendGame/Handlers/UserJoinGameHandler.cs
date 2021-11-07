using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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
        public async Task Handle(UserJoinGame notification, CancellationToken cancellationToken)
        {
            var game = await _db.Games
                .Include(g => g.WaitingPool)
                .Where(g => g.Id.Equals(notification.Entry.GameId))
                .FirstAsync(cancellationToken: cancellationToken);
            
            await _hubContext.Clients.All.SendAsync("GameRoomUpdated", game, cancellationToken: cancellationToken);
        }
    }
}