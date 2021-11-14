using backend.Core.Domain.BackendGame.Models;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record GameTerminated: BaseDomainEvent
    {
        public Game Game { get; }

        public GameTerminated(Game game)
        {
            Game = game;
        }
    }
}