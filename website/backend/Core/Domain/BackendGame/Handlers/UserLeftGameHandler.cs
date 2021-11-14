using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Pipelines;
using backend.Core.Domain.BackendGame.Services;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Handlers
{
    public class UserLeftGameHandler: INotificationHandler<UserLeftGame>
    {
        private readonly GameContext _db;
        private readonly IHubContext<GamesHub> _hubContext;
        private readonly IMediator _mediator;
        private readonly IBackendGameService _backendGameService;

        public UserLeftGameHandler(GameContext db, IHubContext<GamesHub> hubContext, IMediator mediator, IBackendGameService backendGameService)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _hubContext = hubContext;
            _mediator = mediator;
            _backendGameService = backendGameService;
        }
        
        public async Task Handle(UserLeftGame domainEvent, CancellationToken cancellationToken)
        {
       
            await _hubContext.Clients.All.SendAsync("GameRoomUpdated", domainEvent.Notification, cancellationToken);
            
            var game = await _db.Games.FirstAsync(g => g.Id == domainEvent.Notification.GameId, cancellationToken);
            var slotInfo = _backendGameService.GetSlotInfo(game);

            if (!slotInfo.GuessersIds.Any())
            {
                await _mediator.Send(new TerminateGame.Request(game), cancellationToken);
                return;
            }

            if (slotInfo.ProposerId.Equals(domainEvent.UserId))
            {
                // update proposer in game context
            }
        }
    }
}