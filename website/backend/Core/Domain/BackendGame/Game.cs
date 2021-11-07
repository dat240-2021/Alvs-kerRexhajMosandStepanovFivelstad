using System;
using System.Collections.Generic;
using backend.Core.Domain.BackendGame.Events;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class Game: BaseEntity
    {
        public Guid Id { get; protected set; }
        public GameSettings Settings { get; protected set; }

        public GameState State { get; protected set; } = GameState.Created;
        
        public List<WaitingEntry> WaitingPool { get; protected set; }
        
        public Game() { }
        public Game(Guid id, GameSettings settings)
        {
            Id = id;
            Settings = settings;
            Events.Add(new GameCreated(id));
        }

    }
}