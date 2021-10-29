using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Domain.Authentication.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

