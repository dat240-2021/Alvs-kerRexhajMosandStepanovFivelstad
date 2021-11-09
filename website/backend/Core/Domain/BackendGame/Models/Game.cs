using System;
using backend.Core.Domain.BackendGame.Pipelines;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Models
{
    public class Game : BaseEntity
    {
        public Guid Id { get; protected set; }
        public GameSettings Settings { get; protected set; }

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