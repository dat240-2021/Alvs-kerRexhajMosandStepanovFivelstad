using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Events;
using backend.Core.Domain.Lobby.Services;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Lobby.Handlers
{
    public class StartGameHandler : INotificationHandler<GameStarted>
    {
        private readonly GameContext _db;
        private readonly IMediator _mediator;
        private readonly ILobbyService _lobbyService;
        private readonly IHubContext<LobbyHub> _hubContext;

        public StartGameHandler(GameContext db, IMediator mediator, ILobbyService lobbyService, IHubContext<LobbyHub> hubContext)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _mediator = mediator ?? throw new System.ArgumentException(nameof(mediator));
            _lobbyService = lobbyService ?? throw new System.ArgumentException(nameof(LobbyService));
            _hubContext = hubContext ?? throw new System.ArgumentException(nameof(hubContext));
        }


        public async Task Handle(GameStarted notification, CancellationToken cancellationToken)
        {
            var playerIds = _lobbyService.GetSlotInfo(notification.Game.Id).PlayerIds;
            await _hubContext.Clients.Users(playerIds.Select(id => id.ToString())).SendAsync("GameStarted", notification.Game.Id, cancellationToken);
        }
    }
}