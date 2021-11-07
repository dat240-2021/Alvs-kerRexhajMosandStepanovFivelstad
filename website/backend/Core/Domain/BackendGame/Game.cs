using System;
using System.Collections.Generic;
using System.Linq;
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

        public void RemoveUserByIdFromWaitingPool(Guid id)
        {
            var entry = WaitingPool.First(p => p.UserId.Equals(id));
            WaitingPool.Remove(entry);
            Events.Add(new UserLeftGame(entry));
        }
    }
}