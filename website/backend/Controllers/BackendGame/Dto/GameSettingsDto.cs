using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.BackendGame.Dto
{
    public record GameSettingsDto
    {
        [Required]
        public int PlayersCount { get; set;}

        [Required]
        public int ImagesCount { get; set;}
        
        [Required]
        public int Duration { get; set;}
    }
}