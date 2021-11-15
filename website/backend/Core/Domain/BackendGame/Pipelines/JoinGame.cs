using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Services;
using Domain.Authentication;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class JoinGame
    {
        public record Request(User User, Guid GameId, SlotRole Role): IRequest<Unit> {}
        
        public class Handler: IRequestHandler<Request, Unit>
        {

            private readonly GameContext _db;
            private readonly IBackendGameService _backendGameService;
            private readonly IMediator _mediator;

            public Handler(GameContext db, IBackendGameService backendGameService, IMediator mediator)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _backendGameService = backendGameService;
                _mediator = mediator;
            }

            
            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = await _db.Games.FirstOrDefaultAsync(g => g.Id.Equals(request.GameId), cancellationToken) ?? throw new Exception($"Game with id {request.GameId} not found");
                await _backendGameService.JoinGame(game, request.User.Id, request.Role);
                var gameSlotInfo = _backendGameService.GetSlotInfo(game);
                await _mediator.Publish(new UserJoinGame(new GameSlotNotification(game.Id, gameSlotInfo.GuessersIds.Count)), cancellationToken);

                return Unit.Value;
            }
        }
    }
}