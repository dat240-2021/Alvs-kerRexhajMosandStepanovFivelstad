using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Models
{
    [Owned]
    public record GameSettings
    {
        public int PlayersCount { get; set;}
        
        public int ImagesCount { get; set;}
        
        public int Duration { get; set;}
    }
}