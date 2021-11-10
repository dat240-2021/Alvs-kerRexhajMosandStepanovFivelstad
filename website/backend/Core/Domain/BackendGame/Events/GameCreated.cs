using backend.Core.Domain.BackendGame.Models;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record GameCreated : BaseDomainEvent
    {
        public Game Game { get; }
        public int OccupiedSlotsCount { get; } = 0;

        public GameCreated(Game game)
        {
            Game = game;
        }
    }
}