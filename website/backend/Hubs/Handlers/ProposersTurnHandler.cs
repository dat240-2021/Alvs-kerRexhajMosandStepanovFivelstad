
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.GameSpace.Events;
using MediatR;
using System;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs.Handlers
{
    public class ProposersTurnHandler: INotificationHandler<ProposersTurnEvent>
    {
        private readonly IHubContext<GameHub> _hub;

        public ProposersTurnHandler(IHubContext<GameHub> hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        public async Task Handle(ProposersTurnEvent notification, CancellationToken cancellationToken)
        {
            await _hub.Clients.Clients(notification.ProposerId).SendAsync("Propose", cancellationToken);
        }
    }
}