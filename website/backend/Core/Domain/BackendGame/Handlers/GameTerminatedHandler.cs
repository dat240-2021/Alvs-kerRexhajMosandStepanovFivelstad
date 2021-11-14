using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.BackendGame.Handlers
{
    public class GameTerminatedHandler : INotificationHandler<GameTerminated>
    {
        private readonly IHubContext<GamesHub> _hubContext;

        public GameTerminatedHandler(IHubContext<GamesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(GameTerminated notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("GameTerminated", notification.Game, cancellationToken);
        }
    }
}