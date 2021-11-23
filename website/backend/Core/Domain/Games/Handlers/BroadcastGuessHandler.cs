using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Events;
using backend.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Games.Handlers
{
    public class BroadcastGuessHandler: INotificationHandler<BroadcastGuessEvent>
    {
        private IHubContext<GameHub> _hub;

        public BroadcastGuessHandler(IHubContext<GameHub> hub)
        {
            _hub = hub;
        }

        public async Task Handle(BroadcastGuessEvent notification, CancellationToken cancellationToken)
        {
            var guess = new GuessResponseDto(){
                Guess = notification.Guess,
                User = notification.Username,
            };
            await _hub.Clients.Users(notification.PlayerIds).SendAsync("Guess", guess, cancellationToken);
        }
    }
}