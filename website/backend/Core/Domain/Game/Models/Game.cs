//This is placed here temporarily, will be moved if needed. 
using System;
using Domain.Image;

namespace backend.Core.Domain.Game {
    public record Game {
        public int GameId { get; protected set; }
        public IProposer Proposer { get; protected set; }

        public List<Guesser> Guessers { get; protected set; }

        public List<Image> Images { get; protected set; }

        public TimeSpan RoundTime { get; protected set; }

        public Game() {}
    }
}