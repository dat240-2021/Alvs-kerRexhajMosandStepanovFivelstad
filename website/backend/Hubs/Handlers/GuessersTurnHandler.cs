
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Events;
using MediatR;
using System;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs.Handlers
{
    public class GuessersTurnHandler : INotificationHandler<GuessersTurnEvent>
    {
        private readonly IHubContext<GameHub> _hub;

        public GuessersTurnHandler(IHubContext<GameHub> hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        public async Task Handle(GuessersTurnEvent notification, CancellationToken cancellationToken)
        {
            await _hub.Clients.Users(notification.PlayerIds).SendAsync("GuessersTurn", cancellationToken);
        }
    }
}