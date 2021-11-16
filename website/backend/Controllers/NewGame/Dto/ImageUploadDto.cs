using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.NewGame.Dto
{
    public record ImageUploadDto
    {
        [Required]
        public ImageUploaded[] ImageList { get; set; }
    }


    public record ImageUploaded {
        [Required]
        public int id;
        [Required]
        public string name;
        [Required]
        public int category;
        [Required]
        public string label;
        [Required]
        public string file;
    }

}