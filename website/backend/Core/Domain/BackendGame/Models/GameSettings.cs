using Microsoft.EntityFrameworkCore;
using Domain.Image;
using System.Collections.Generic;

namespace backend.Core.Domain.BackendGame.Models
{
    [Owned]
    public record GameSettings
    {
        public Proposer Proposer { get; set; }
        public List<Guesser> Guessers { get; set; }
        
        public List<Image> Images { get; set; }
        
        public int Duration { get; set;}
    }
}