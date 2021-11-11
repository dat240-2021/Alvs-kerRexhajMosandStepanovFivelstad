//This is placed here temporarily, will be moved if needed. 
using System;
using System.Collections.Generic;
using Domain.Image;

namespace backend.Core.Domain.GameSpace {
    public record GameDto {
        public int GameId { get; protected set; }
        public Proposer Proposer { get; protected set; }

        public List<Guesser> Guessers { get; protected set; }

        public List<Image> Images { get; protected set; }

        public TimeSpan RoundTime { get; protected set; }

    }
}