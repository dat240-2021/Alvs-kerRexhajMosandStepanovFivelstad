using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Lobby.Dto
{
    public record LeaderboardScoreDto
    {
        [Required]
        public string Playername { get; set; }

        [Required]
        public int Score { get; set;}
    }
}