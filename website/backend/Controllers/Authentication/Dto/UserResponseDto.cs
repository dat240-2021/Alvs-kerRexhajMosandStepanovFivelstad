using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Controllers.Authentication.Dto
{
    public record UserResponseDto
    {
        [Required] public string Username { get; set; }

        public static implicit operator UserResponseDto(string username)
        {
            return new (){ Username = username };
        }

    }

}

