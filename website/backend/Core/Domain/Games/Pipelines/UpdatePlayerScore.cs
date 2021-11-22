using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Events;
using backend.Core.Domain.Lobby.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace backend.Core.Domain.Games.Pipelines
{
    public class UpdatePlayerScore
    {
        public record Request(Guid UserId, int Score): IRequest<Unit> {}
        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly GameContext _db;
            private readonly IGameService _service;

            public Handler(GameContext db, IGameService service)
            {
                _db = db;
                _service = service;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {

                var dbScore = await _db.Scores.Where(x => x.User == request.UserId).FirstOrDefaultAsync();
                if (dbScore==null){
                    dbScore = new Score(
                        request.UserId,
                        request.Score);
                    _db.Add(dbScore);
                }

                dbScore.UserScore+= request.Score;
                await _db.SaveChangesAsync();

                return Unit.Value;



            }
        }
    }
}