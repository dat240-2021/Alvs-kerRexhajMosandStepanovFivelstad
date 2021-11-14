using backend.Core.Domain.BackendGame.Models;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record GameReadyToStart: BaseDomainEvent
    {
        public Game Game { get; set; }

        public GameReadyToStart(Game game)
        {
            Game = game;
        }
    }
}