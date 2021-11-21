using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Dtos;
using backend.Core.Domain.Games.Events;
using backend.Core.Domain.Lobby.Events;
using backend.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Games.Handlers
{
    public class CorrectGuessHandler: INotificationHandler<CorrectGuessEvent>
    {
        private readonly IHubContext<GameHub> _hubContext;

        public CorrectGuessHandler(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task Handle(CorrectGuessEvent notification, CancellationToken cancellationToken)
        {
            var correctGuess = new CorrectGuessDto()
            {
                Guess = notification.Guess, 
                Image = notification.Game.CurrentImage, 
                UserId = notification.UserId,
                HasMoreRounds = notification.HasMoreRounds,
                WillAutoContinue = notification.WillAutoContinue
            };
            await _hubContext.Clients.Users(notification.Game.PlayerIds).SendAsync("CorrectGuess", correctGuess, cancellationToken);
        }
    }
}