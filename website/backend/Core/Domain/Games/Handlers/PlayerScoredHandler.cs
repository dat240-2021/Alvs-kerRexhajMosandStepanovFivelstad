using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using backend.Core.Domain.Games.Events;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.SignalR;
using backend.Hubs;
using backend.Core.Domain.Games.Pipelines;

namespace backend.Core.Domain.Games.Handlers
{
    public class PlayerScoredHandler: INotificationHandler<PlayerScoredEvent>
    {
        private readonly GameContext _db;
        private readonly IGameService _service;

        private readonly IHubContext<GameHub> _hub;

        public PlayerScoredHandler(GameContext db, IGameService service, IHubContext<GameHub> hub)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _service = service ?? throw new System.ArgumentException(nameof(service));
            _hub = hub ?? throw new System.ArgumentException(nameof(hub));
        }

        public async Task Handle(PlayerScoredEvent notification, CancellationToken cancellationToken)
        {

            var game = _service.GetByUserId(notification.UserId);

            await _hub.Clients.Clients(game.Guessers.Select(g => g.Id.ToString())).SendAsync("APlayerScored", notification.UserId, notification.Score, cancellationToken);
            var proposer = game.Proposer.GetId();
            if (proposer is not null) {
                await _hub.Clients.Client(proposer).SendAsync("APlayerScored", notification.UserId, notification.Score, cancellationToken);
            }


            var dbscore = await _db.Scores.Include(x => x.User).Where(x => x.User.Id==notification.UserId).FirstOrDefaultAsync();
            dbscore.UserScore+= notification.Score;
            await _db.SaveChangesAsync();

        }
    }
}