using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Controllers.Lobby.Dto;
using backend.Core.Domain.Lobby.Models;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Lobby.Pipelines
{
    public class GetLeaderboard
    {
        public record Request() : IRequest<List<LeaderboardScoreDto>>;

        public class Handler: IRequestHandler<Request, List<LeaderboardScoreDto>>
        {

            private GameContext _db;

            public Handler(GameContext db)
            {
                _db = db ?? throw new System.ArgumentNullException(nameof(db));
            }

            public async Task<List<LeaderboardScoreDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                var scores = await _db.Scores
                    .OrderBy(s => s.UserScore)
                    .Take(10)
                    .ToArrayAsync(cancellationToken: cancellationToken);

                var leaderboard = new List<LeaderboardScoreDto>();
                foreach( var s in scores){
                    leaderboard.Add(new LeaderboardScoreDto(){
                        Playername = _db.Users.Where( u => u.Id == s.User ).Select( u => u.UserName).FirstOrDefault(),
                        Score = s.UserScore,
                    });
                }
                return leaderboard;
            }
        }
    }
}