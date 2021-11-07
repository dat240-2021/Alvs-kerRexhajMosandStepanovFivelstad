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
    public class GetWaitingGames
    {
        public record Request(): IRequest<List<Game>> {}

        public class Handler: IRequestHandler<Request, List<Game>>
        {

            private readonly GameContext _db;

            public Handler(GameContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }

            
            public async Task<List<Game>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _db.Games
                    .Include(g => g.WaitingPool)
                    .Where(g => g.State == GameState.Created)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}

