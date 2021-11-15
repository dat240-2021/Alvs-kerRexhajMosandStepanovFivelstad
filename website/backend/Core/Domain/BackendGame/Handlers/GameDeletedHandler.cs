using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using backend.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.BackendGame.Handlers
{
    public class GameDeletedHandler : INotificationHandler<GameDeleted>
    {
        private readonly IHubContext<GamesHub> _hubContext;

        public GameDeletedHandler(IHubContext<GamesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(GameDeleted notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("GameDeleted", notification.Game.Id, cancellationToken);
        }
    }
}