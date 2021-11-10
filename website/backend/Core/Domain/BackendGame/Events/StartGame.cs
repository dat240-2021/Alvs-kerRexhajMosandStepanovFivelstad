using backend.Core.Domain.BackendGame.Models;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record StartGame: BaseDomainEvent
    {
        public Game Game { get; set; }

        public StartGame(Game game)
        {
            Game = game;
        }
    }
}