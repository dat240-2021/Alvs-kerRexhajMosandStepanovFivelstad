using backend.Core.Domain.BackendGame.Models;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record GameStarted: BaseDomainEvent
    {
        public Game Game { get; set; }

        public GameStarted(Game game)
        {
            Game = game;
        }
    }
}