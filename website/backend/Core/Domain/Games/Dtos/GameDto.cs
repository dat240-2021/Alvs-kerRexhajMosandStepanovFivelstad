//This is placed here temporarily, will be moved if needed. 
using System;
using System.Collections.Generic;
using backend.Core.Domain.Images;

namespace backend.Core.Domain.Games
{
    public record GameDto
    {
        public int GameId { get; protected set; }
        public Proposer Proposer { get; protected set; }

        public List<Guesser> Guessers { get; protected set; }

        public List<Images.Image> Images { get; protected set; }

        public TimeSpan RoundTime { get; protected set; }

    }
}