using backend.Core.Domain.BackendGame.Models;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record GameDeleted: BaseDomainEvent
    {
        public Game Game { get; }

        public GameDeleted(Game game)
        {
            Game = game;
        }
    }
}