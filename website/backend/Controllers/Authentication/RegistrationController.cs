
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using Domain.Authentication.Pipelines;
using Controllers.Authentication.Dto;
using Controllers.Generics;

namespace Controllers.Authentication
{
    [ApiController]
    [Route("/api/register")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IMediator _mediator;

        public RegistrationController(ILogger<RegistrationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;

        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDto user){
            var result = await _mediator.Send(new RegisterUser.Request(user.Username,user.Password));

            if (result.Success){
                return Ok(new GenericResponseObject<UserResponseDto>{
                    Data = new UserResponseDto{
                        Username = user.Username
                        }
                    });
            }

            return UnprocessableEntity(new GenericResponseObject<UserResponseDto>{Errors = result.errors});
        }
    }



}
