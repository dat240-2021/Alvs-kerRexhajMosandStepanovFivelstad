using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class GetLeaderboard
    {
        public record Request() : IRequest<List<Score>>;

        public class Handler: IRequestHandler<Request, List<Score>>
        {

            private GameContext _db;

            public Handler(GameContext db)
            {
                _db = db ?? throw new System.ArgumentNullException(nameof(db));
            }

            
            public async Task<List<Score>> Handle(Request request, CancellationToken cancellationToken)
            {
     
                return await _db.Scores
                    .OrderBy(s => s.UserScore)
                    .Take(10)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}