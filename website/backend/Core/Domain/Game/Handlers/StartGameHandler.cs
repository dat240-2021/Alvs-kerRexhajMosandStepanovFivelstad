using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Game.Events;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.Game.Handlers
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
                Proposer = notification.ProposerId is not null ? new Proposer(notification.ProposerId) : new Oracle(),
                Guessers = notification.GuesserIds.Select(g => new Guesser(g)),
                Images = await _db.Images.Where(i => notification.ImageIds.Contains(i.Id)).ToListAsync(),
                RoundTime = notification.RoundTime,
            };

            _service.Add(game);
        }
    }
}