using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Services;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class DeleteGame
    {
        public record Request(Game Game): IRequest<Unit> {}
        
        public class Handler: IRequestHandler<Request, Unit>
        {
            private readonly GameContext _db;
            private readonly IMediator _mediator;
            private readonly IBackendGameService _backendGameService;

            public Handler(GameContext db, IMediator mediator, IBackendGameService backendGameService)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
                _backendGameService = backendGameService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _db.Games.Remove(request.Game);
                await _db.SaveChangesAsync(cancellationToken);
                _backendGameService.deleteGame(request.Game.Id);
                await _mediator.Publish(new GameDeleted(request.Game), cancellationToken);
                return Unit.Value;
            }
        }
    }
}