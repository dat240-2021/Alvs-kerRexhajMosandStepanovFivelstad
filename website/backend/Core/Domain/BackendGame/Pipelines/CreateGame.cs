using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Models;
using Domain.Authentication;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class CreateGame
    {
        public record Request(GameSettings GameSettings, User User): IRequest<Guid> {}

        public class Handler: IRequestHandler<Request, Guid>
        {

            private GameContext _db;
            private readonly IMediator _mediator;

            public Handler(GameContext db, IMediator mediator)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
            }

            
            public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = new Game(Guid.NewGuid(), request.GameSettings, request.User);
                var creatorRole = request.GameSettings.ProposerType == "AI" ? SlotRole.Guesser : SlotRole.Proposer;
                
                _db.Games.Add(game);
                
                await _db.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new GameCreated(game), cancellationToken);
                await _mediator.Send(new JoinGame.Request(request.User, game.Id, creatorRole), cancellationToken);
                return game.Id;
            }
        }
    }
}