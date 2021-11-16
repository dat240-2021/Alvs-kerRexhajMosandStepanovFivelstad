using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.NewGame.Dto
{
    public record ImageUploadDto
    {
        [Required]
        public ImageFile[] ImageList { get; set; }
    }


    public record ImageFile {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Category { get; set; }
        [Required]
        public string Label { get; set; }
        [Required]
        public string File { get; set; }
    }

}