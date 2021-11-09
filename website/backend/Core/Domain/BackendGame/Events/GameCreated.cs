using System;
using backend.Core.Domain.BackendGame.Models;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record GameCreated : BaseDomainEvent
    {
        public Game Game { get; }

        public GameCreated(Game game)
        {
            Game = game;
        }
    }
}