using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Authentication.Dto
{
    public record UserResponseDto
    {
        [Required] public string Username { get; set; }

        public UserResponseDto(string username)
        {
            Username = username;
        }

    }

}

