using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Events;
using backend.Core.Domain.Lobby.Models;
using backend.Core.Domain.Lobby.Services;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.Lobby.Pipelines
{
    public class DeleteGame
    {
        public record Request(Game Game): IRequest<Unit> {}
        
        public class Handler: IRequestHandler<Request, Unit>
        {
            private readonly GameContext _db;
            private readonly IMediator _mediator;
            private readonly ILobbyService _LobbyService;

            public Handler(GameContext db, IMediator mediator, ILobbyService LobbyService)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
                _LobbyService = LobbyService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _db.Games.Remove(request.Game);
                await _db.SaveChangesAsync(cancellationToken);
                _LobbyService.DeleteGame(request.Game.Id);
                await _mediator.Publish(new GameDeleted(request.Game), cancellationToken);
                return Unit.Value;
            }
        }
    }
}