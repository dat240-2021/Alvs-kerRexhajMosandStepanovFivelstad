using System.Threading.Tasks;
using backend.Controllers.Authentication.Dto;
using Controllers.Authentication.Dto;
using Controllers.Generics;
using Domain.Authentication.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers.Authentication
{
    [ApiController]
    [Route("/api/register")]
    public class RegistrationController : ApiBaseController
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IMediator _mediator;

        public RegistrationController(ILogger<RegistrationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;

        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDto user)
        {
            var result = await _mediator.Send(new RegisterUser.Request(user.Username, user.Password));

            if (result.Success)
            {
                return Ok(new UserResponseDto(user.Username));
            }

            return UnprocessableEntity(result.errors);
        }
    }



}
