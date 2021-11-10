using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.BackendGame.Handlers
{
    public class StartGameHandler: INotificationHandler<StartGame>
    {
        private readonly GameContext _db;
        private readonly IMediator _mediator;

        public StartGameHandler(GameContext db, IMediator mediator)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _mediator = mediator;
        }
        
        public async Task Handle(StartGame notification, CancellationToken cancellationToken)
        {
            await _mediator.Send(new Pipelines.StartGame.Request(notification.Game), cancellationToken);
        }
    }
}