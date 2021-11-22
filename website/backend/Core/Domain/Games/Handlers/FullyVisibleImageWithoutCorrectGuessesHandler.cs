using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Events;
using backend.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Games.Handlers
{
    public class FullyVisibleImageWithoutCorrectGuessesHandler: INotificationHandler<FullyVisibleImageWithoutCorrectGuessesEvent>
    {
        private readonly IGameService _service;
        private IHubContext<GameHub> _hub;

        public FullyVisibleImageWithoutCorrectGuessesHandler(IGameService service, IHubContext<GameHub> hub)
        {
            _service = service;
            _hub = hub;
        }

        public async Task Handle(FullyVisibleImageWithoutCorrectGuessesEvent notification, CancellationToken cancellationToken)
        {
            var game = _service.Get(notification.GameId);
            await _hub.Clients.Users(game.PlayerIds).SendAsync("ImageFullyVisibleWithNoCorrectGuesses", notification.Guess, cancellationToken);
        }
    }
}