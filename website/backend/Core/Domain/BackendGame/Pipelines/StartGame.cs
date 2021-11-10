using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class StartGame
    {
        public record Request(Game Game): IRequest<Unit> {}
        
        public class Handler: IRequestHandler<Request, Unit>
        {

            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                request.Game.State = GameState.Active;
                await _db.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}