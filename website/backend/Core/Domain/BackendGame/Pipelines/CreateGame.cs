using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Authentication;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class CreateGame
    {
        public record Request(GameSettings GameSettings, Guid UserId): IRequest<Unit> {}

        public class Handler: IRequestHandler<Request, Unit>
        {

            private GameContext _db;

            public Handler(GameContext db)
            {
                _db = db ?? throw new System.ArgumentNullException(nameof(db));
            }

            
            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _db.Users.FirstAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

                var game = new Game(Guid.NewGuid(), request.GameSettings);
                _db.Games.Add(game);
                
                await _db.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}