using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class LeaveGame
    {
        public record Request(Guid UserId, Guid GameId): IRequest<Unit> {}
        
        public class Handler: IRequestHandler<Request, Unit>
        {

            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = await _db.Games.Include(g => g.WaitingPool).FirstAsync(g => g.Id == request.GameId);
                game.RemoveUserByIdFromWaitingPool(request.UserId);
                await _db.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}