using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Models;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class StartGame
    {
        public record Request(Guid GameId, Guid? UserId = null): IRequest<Response> {}
        
        public record Response(bool Success);

        public class Handler: IRequestHandler<Request, Response>
        {

            private readonly GameContext _db;
            private readonly IMediator _mediator;

            public Handler(GameContext db, IMediator mediator)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = await _db.Games.Include(g => g.Creator).FirstOrDefaultAsync(g => g.Id.Equals(request.GameId), cancellationToken);
                if (request.UserId is not null && request.UserId != game.Creator.Id)
                {
                    return new Response(false);
                }
                game.State = GameState.Active;
                await _db.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new GameStarted(game), cancellationToken);
                return new Response(true);
            }
        }
    }
}