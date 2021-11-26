using backend.Core.Domain.Lobby.Models;
using SharedKernel;

namespace backend.Core.Domain.Lobby.Events
{
    public record GameStarted : BaseDomainEvent
    {
        public Game Game { get; set; }

        public GameStarted(Game game)
        {
            Game = game;
        }
    }
}