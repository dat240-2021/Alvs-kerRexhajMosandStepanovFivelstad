using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Controllers.Authentication.Models
{
    public record UserReceiveDto
    {
        [Required]
        public string Username { get; }

        [Required]
        public string Password { get; }
    }


    public record UserSendDto
    {
        [Required]
        public string Username { get; set; }

    }


}

