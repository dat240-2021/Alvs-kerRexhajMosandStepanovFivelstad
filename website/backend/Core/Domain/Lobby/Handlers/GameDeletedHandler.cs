using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Events;
using backend.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Lobby.Handlers
{
    public class GameDeletedHandler : INotificationHandler<GameDeleted>
    {
        private readonly IHubContext<LobbyHub> _hubContext;

        public GameDeletedHandler(IHubContext<LobbyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(GameDeleted notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("GameDeleted", notification.Game.Id, cancellationToken);
        }
    }
}