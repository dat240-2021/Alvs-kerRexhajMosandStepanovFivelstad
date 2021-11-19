using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Events;
using backend.Core.Domain.Lobby.Pipelines;
using backend.Core.Domain.Lobby.Services;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Lobby.Handlers
{
    public class UserLeftGameHandler: INotificationHandler<UserLeftGame>
    {
        private readonly GameContext _db;
        private readonly IHubContext<GamesHub> _hubContext;
        private readonly IMediator _mediator;
        private readonly ILobbyService _LobbyService;

        public UserLeftGameHandler(GameContext db, IHubContext<GamesHub> hubContext, IMediator mediator, ILobbyService LobbyService)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _hubContext = hubContext;
            _mediator = mediator;
            _LobbyService = LobbyService;
        }
        
        public async Task Handle(UserLeftGame domainEvent, CancellationToken cancellationToken)
        {
       
            await _hubContext.Clients.All.SendAsync("GameRoomUpdated", domainEvent.Notification, cancellationToken);
            
            var game = await _db.Games.Include(g => g.Creator).FirstAsync(g => g.Id == domainEvent.Notification.GameId, cancellationToken);

            if (game.Creator.Id.Equals(domainEvent.UserId))
            {
                await _mediator.Send(new DeleteGame.Request(game), cancellationToken);
            }
        }
    }
}