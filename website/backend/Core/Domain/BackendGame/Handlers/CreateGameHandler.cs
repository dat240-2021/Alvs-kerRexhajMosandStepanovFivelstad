using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines.Handlers
{
    public class CreateGameHandler: INotificationHandler<GameCreated>
    {
        private readonly GameContext _db;
        private readonly IHubContext<GamesHub> _hubContext;

        public CreateGameHandler(GameContext db, IHubContext<GamesHub> hubContext)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _hubContext = hubContext;
        }
        public async Task Handle(GameCreated notification, CancellationToken cancellationToken)
        {
            var game = await _db.Games.FirstAsync(g => g.Id == notification.Id);
            await _hubContext.Clients.All.SendAsync("GameCreated", game, cancellationToken: cancellationToken);
        }
    }
}