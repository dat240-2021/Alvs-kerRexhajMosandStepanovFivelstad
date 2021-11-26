using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Lobby.Models
{
    [Owned]
    public record GameSettings
    {
        public string ProposerType { get; set; }

        public List<int> CategoryIds { get; set; }

        public int GuessersCount { get; set; }

        public int ImagesCount { get; set; }

        public int Duration { get; set; }
    }
}