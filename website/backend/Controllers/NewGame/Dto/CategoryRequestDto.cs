using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.NewGame.Dto
{
    public record CategoryRequestDto
    {
        [Required]
        public List<string> CategoryList { get; set;}
    }

}