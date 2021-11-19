using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Events;
using backend.Core.Domain.Lobby.Models;
using backend.Core.Domain.Lobby.Services;
using Domain.Authentication;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.Lobby.Pipelines
{
    public class CreateGame
    {
        public record Request(GameSettings GameSettings, User User): IRequest<GameWithSlotInfo> {}

        public class Handler: IRequestHandler<Request, GameWithSlotInfo>
        {

            private GameContext _db;
            private readonly IMediator _mediator;
            private readonly ILobbyService _LobbyService;

            public Handler(GameContext db, IMediator mediator, ILobbyService LobbyService)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
                _LobbyService = LobbyService;
            }

            
            public async Task<GameWithSlotInfo> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = new Game(Guid.NewGuid(), request.GameSettings, request.User);
                var creatorRole = request.GameSettings.ProposerType == "AI" ? SlotRole.Guesser : SlotRole.Proposer;
                
                _db.Games.Add(game);
                
                await _db.SaveChangesAsync(cancellationToken);
                _LobbyService.StoreGame(game);
                await _mediator.Publish(new GameCreated(game), cancellationToken);
                await _mediator.Send(new JoinGame.Request(request.User, game.Id, creatorRole), cancellationToken);
                return new GameWithSlotInfo(game, _LobbyService.GetSlotInfo(game.Id));
            }
        }
    }
}