using Microsoft.EntityFrameworkCore;
using Domain.Image;
using System.Collections.Generic;

namespace backend.Core.Domain.BackendGame.Models
{
    [Owned]
    public record GameSettings
    {
        public int PlayersCount {get; set;}
        public int ImageCount {get; set;}
        public int Duration { get; set;}
    }
}