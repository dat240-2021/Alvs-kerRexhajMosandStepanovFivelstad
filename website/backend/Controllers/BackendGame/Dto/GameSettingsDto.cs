using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.BackendGame.Dto
{
    public record GameSettingsDto
    {
        [Required]
        public string ProposerType { get; set; }
        
        [Required]
        [MinLength(1)]
        public List<int> CategoryIds { get; set; }
        
        [Required]
        public int PlayersCount { get; set;}

        [Required]
        public int ImagesCount { get; set;}
        
        [Required]
        public int RoundDuration { get; set;}
    }
}