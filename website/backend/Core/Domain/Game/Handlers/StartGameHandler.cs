using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using backend.Core.Domain.GameSpace.Events;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace backend.Core.Domain.GameSpace.Handlers
{
    public class StartGameHandler: INotificationHandler<StartGame>
    {
        private readonly GameContext _db;
        private readonly IGameService _service;

        public StartGameHandler(GameContext db, IGameService service)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _service = service ?? throw new System.ArgumentException(nameof(service));
        }

        public async Task Handle(StartGame notification, CancellationToken cancellationToken)
        {
            var game = new Game() {
                Proposer = new Proposer(notification.ProposerId),
                Guessers = notification.GuesserIds.Select(g => new Guesser(g)).ToList(),
                Images = await _db.Images.Where(i => notification.ImageIds.Contains(i.Id)).ToListAsync(),
                RoundTime = notification.RoundTime,
            };

            _service.Add(game);
        }
    }
}