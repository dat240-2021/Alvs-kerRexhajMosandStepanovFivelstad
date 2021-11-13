using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Models
{
    [Owned]
    public record GameSettings
    {
        public string ProposerType { get; set; }
        
        public List<int> CategoryIds { get; set; }
        
        public int PlayersCount { get; set;}
        
        public int ImagesCount { get; set;}
        
        public int Duration { get; set;}
    }
}