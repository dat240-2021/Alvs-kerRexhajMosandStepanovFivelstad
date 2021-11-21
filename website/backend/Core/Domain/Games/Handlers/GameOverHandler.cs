using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using backend.Core.Domain.Games.Events;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using backend.Core.Domain.Games.Dtos;
using Microsoft.AspNetCore.SignalR;
using backend.Hubs;

namespace backend.Core.Domain.Games.Handlers
{
    public class GameOverHandler: INotificationHandler<GameOverEvent>
    {
        private readonly IGameService _service;
        private IHubContext<GameHub> _hub;


        public GameOverHandler(IGameService service,IHubContext<GameHub> hub )
        {
            _service = service ?? throw new System.ArgumentException(nameof(service));
            _hub = hub ?? throw new System.ArgumentException(nameof(hub));
        }

        public async Task Handle(GameOverEvent notification, CancellationToken cancellationToken)
        {
            var game = _service.Remove(notification.GameId);
            var guessersScores = game.Guessers.ToDictionary(guesser => guesser.Id.ToString(), guesser => guesser.Score);


            int? proposerScore = null;
            
            if (!game.VersusOracle)
            {
                proposerScore = game.Proposer.Score;
            }
            
            await _hub.Clients.Users(game.PlayerIds).SendAsync("GameOver", guessersScores, proposerScore, cancellationToken);
        }
    }
}