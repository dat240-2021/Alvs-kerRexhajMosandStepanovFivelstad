using System;
using backend.Core.Domain.BackendGame.Models;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record StartGame: BaseDomainEvent
    {
        public Game Game { get; set; }
        public Guid? PlayerId { get; set; } = null;

        public StartGame(Game game)
        {
            Game = game;
        }
        
        public StartGame(Game game, Guid playerId)
        {
            Game = game;
            PlayerId = playerId;
        }
    }
}