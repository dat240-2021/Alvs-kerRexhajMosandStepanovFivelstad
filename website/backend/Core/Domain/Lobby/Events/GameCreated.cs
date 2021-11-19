using backend.Core.Domain.Lobby.Models;
using SharedKernel;

namespace backend.Core.Domain.Lobby.Events
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