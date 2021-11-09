using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Services;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class LeaveGame
    {
        public record Request(Guid UserId, Guid GameId): IRequest<Unit> {}
        
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
                await _backendGameService.LeaveGame(request.GameId, request.UserId);
                var gameSlotInfo = _backendGameService.GetSlotInfo(request.GameId);
                await _mediator.Publish(new UserLeftGame(new GameSlotNotification(request.GameId, gameSlotInfo.Players.Count)), cancellationToken);
                return Unit.Value;
            }
        }
    }
}