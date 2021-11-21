using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Authentication.Dto
{
    public record UserResponseDto
    {
        [Required] public string Username { get; set; }
        
        public Guid? Id { get; set; }

        public UserResponseDto(Guid? id, string username)
        {
            Username = username;
            Id = id;
        }

    }

}

