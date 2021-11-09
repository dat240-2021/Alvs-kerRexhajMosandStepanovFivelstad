using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Models;
using Domain.Authentication;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class CreateGame
    {
        public record Request(GameSettings GameSettings, Guid UserId): IRequest<Guid> {}

        public class Handler: IRequestHandler<Request, Guid>
        {

            private GameContext _db;
            private readonly IMediator _mediator;

            public Handler(GameContext db, IMediator mediator)
            {
                _db = db ?? throw new System.ArgumentNullException(nameof(db));
                _mediator = mediator;
            }

            
            public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = new Game(Guid.NewGuid(), request.GameSettings);
                _db.Games.Add(game);
                await _db.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new GameCreated(game), cancellationToken);
                
                await _mediator.Send(new JoinGame.Request(request.UserId, game.Id), cancellationToken);
                return game.Id;
            }
        }
    }
}