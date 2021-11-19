using backend.Core.Domain.Lobby.Models;
using SharedKernel;

namespace backend.Core.Domain.Lobby.Events
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