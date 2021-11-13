using System;

namespace backend.Core.Domain.BackendGame.Models
{
    public class Game
    {
        public Guid Id { get; protected set; }
        public GameSettings Settings { get; set; }

        public GameState State { get; set; } = GameState.Created;
        
        public Game()
        {
            
        }

        public Game(Guid id, GameSettings settings)
        {
            Id = id;
            Settings = settings;
        }
    }
}