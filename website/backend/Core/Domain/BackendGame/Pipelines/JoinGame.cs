using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class JoinGame
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

                var user = await _db.Users.FirstAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

                var entry = new WaitingEntry(user.Id, request.GameId);
                _db.WaitingPool.Add(entry);

                await _db.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}