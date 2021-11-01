using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Controllers.Authentication.Models
{
    public record UserRequestDto
    {
        [Required]
        public string Username { get; set;}

        [Required]
        public string Password { get; set;}
    }


    public record UserResponseDto
    {
        [Required]
        public string Username { get; set; }

    }


}

