using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Services;
using Domain.Authentication;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class CreateGame
    {
        public record Request(GameSettings GameSettings, User User): IRequest<GameWithSlotInfo> {}

        public class Handler: IRequestHandler<Request, GameWithSlotInfo>
        {

            private GameContext _db;
            private readonly IMediator _mediator;
            private readonly IBackendGameService _backendGameService;

            public Handler(GameContext db, IMediator mediator, IBackendGameService backendGameService)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
                _backendGameService = backendGameService;
            }

            
            public async Task<GameWithSlotInfo> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = new Game(Guid.NewGuid(), request.GameSettings, request.User);
                var creatorRole = request.GameSettings.ProposerType == "AI" ? SlotRole.Guesser : SlotRole.Proposer;
                
                _db.Games.Add(game);
                
                await _db.SaveChangesAsync(cancellationToken);
                _backendGameService.StoreGame(game);
                await _mediator.Publish(new GameCreated(game), cancellationToken);
                await _mediator.Send(new JoinGame.Request(request.User, game.Id, creatorRole), cancellationToken);
                return new GameWithSlotInfo(game, _backendGameService.GetSlotInfo(game.Id));
            }
        }
    }
}