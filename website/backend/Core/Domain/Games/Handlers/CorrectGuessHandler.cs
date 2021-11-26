using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Dtos;
using backend.Core.Domain.Games.Events;
using backend.Core.Domain.Lobby.Events;
using backend.Core.Domain.Lobby.Models;
using backend.Hubs;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Core.Domain.Games.Handlers
{
    public class CorrectGuessHandler : INotificationHandler<CorrectGuessEvent>
    {
        private readonly IServiceScopeFactory _scope;
        private readonly IHubContext<GameHub> _hubContext;

        public CorrectGuessHandler(IHubContext<GameHub> hubContext, IServiceScopeFactory scope)
        {
            _scope = scope ?? throw new System.ArgumentException(nameof(scope));
            _hubContext = hubContext;
        }

        public async Task Handle(CorrectGuessEvent notification, CancellationToken cancellationToken)
        {

            var playersToScore = new List<(Guid id, int score)>();
            playersToScore.Add((notification.Guesser.Id, notification.GuesserScored));

            if (notification.Proposer is Proposer)
            {
                playersToScore.Add((Guid.Parse(notification.Proposer.GetId()), notification.ProposerScored));
            }

            using (var scope = _scope.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<GameContext>();

                foreach (var player in playersToScore)
                {
                    var score = await db.Scores
                        .Where(x => x.User == player.id)
                        .FirstOrDefaultAsync();

                    if (score is null)
                    {
                        score = new Score(player.id, 0);
                        db.Add(score);
                    }

                    score.UserScore += player.score;
                }

                await db.SaveChangesAsync();
            }

            var correctGuess = new CorrectGuessDto()
            {
                Guess = notification.Guess,
                Image = notification.Image,
                Guesser = notification.Guesser.Username,
                NewGuesserScore = notification.Guesser.Score,
                NewProposerScore = notification.Proposer.Score,
                HasMoreRounds = notification.HasMoreRounds,
                WillAutoContinue = notification.WillAutoContinue
            };

            await _hubContext.Clients.Users(notification.PlayerIds).SendAsync("CorrectGuess", correctGuess, cancellationToken);
        }
    }
}