using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Events;
using backend.Core.Domain.Lobby.Models;
using backend.Core.Domain.Lobby.Services;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Lobby.Pipelines
{
    public class LeaveGame
    {
        public record Request(Guid UserId, Guid GameId) : IRequest<Unit> { }

        public class Handler : IRequestHandler<Request, Unit>
        {

            private readonly GameContext _db;
            private readonly ILobbyService _LobbyService;
            private readonly IMediator _mediator;

            public Handler(GameContext db, ILobbyService LobbyService, IMediator mediator)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _LobbyService = LobbyService;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = await _db.Games.Where(g => g.Id.Equals(request.GameId)).FirstOrDefaultAsync(cancellationToken) ?? throw new Exception($"Game with id {request.GameId} not found");
                _LobbyService.LeaveGame(game.Id, request.UserId);
                var gameSlotInfo = _LobbyService.GetSlotInfo(game.Id);
                await _mediator.Publish(new UserLeftGame(new GameSlotNotification(game.Id, gameSlotInfo.PlayerSlots.Count), request.UserId), cancellationToken);
                return Unit.Value;
            }
        }
    }
}