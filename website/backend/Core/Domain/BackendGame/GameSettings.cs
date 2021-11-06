using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    [Owned]
    public record GameSettings
    {
        public int PlayersCount { get; set;}
        
        public int ImagesCount { get; set;}
        
        public int Duration { get; set;}
    }
}