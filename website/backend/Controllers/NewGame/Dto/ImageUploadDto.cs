using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.NewGame.Dto
{
    public record ImageUploadDto
    {
        [Required]
        public List<(byte[],int)> ImageList { get; set;}
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string ImageName { get; set; }
        
    }

}