using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Models;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class TerminateGame
    {
        public record Request(Game Game): IRequest<Unit> {}
        
        public class Handler: IRequestHandler<Request, Unit>
        {
            private readonly GameContext _db;
            private readonly IMediator _mediator;

            public Handler(GameContext db, IMediator mediator)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                request.Game.State = GameState.Finished;
                await _db.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new GameTerminated(request.Game), cancellationToken);
                return Unit.Value;
            }
        }
    }
}