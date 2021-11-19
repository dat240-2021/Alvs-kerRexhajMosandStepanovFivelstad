using System;
using Domain.Authentication;

namespace backend.Core.Domain.Lobby.Models
{
    public class Game
    {
        public Guid Id { get; protected set; }
        public GameSettings Settings { get; set; }

        public GameState State { get; set; } = GameState.Created;

        public User Creator { get; set; }
        

        public Game()
        {
        }

        public Game(Guid id, GameSettings settings, User creator)
        {
            Id = id;
            Settings = settings;
            Creator = creator;
        }
    }
}