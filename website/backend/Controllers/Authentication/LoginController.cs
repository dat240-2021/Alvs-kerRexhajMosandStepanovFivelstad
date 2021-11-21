using System.Threading.Tasks;
using backend.Controllers.Authentication.Dto;
using Controllers.Authentication.Dto;
using Controllers.Generics;
using Domain.Authentication.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // using Microsoft.AspNetCore.Http.StatusCode;

namespace backend.Controllers.Authentication
{
    
    // [Route("[controller]")]
    [Route("/api/login")]
    public class LoginController : ApiBaseController
    {
        private readonly ILogger<LoginController> _logger;
		private readonly IMediator _mediator;

        public LoginController(ILogger<LoginController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDto user)
        {
            if ( (await _mediator.Send(new LoginUser.Request(user.Username,user.Password))).Success )
            {
                var currentUser = await _mediator.Send(new GetCurrentUser.Request());
                return Ok(new UserResponseDto(currentUser?.Id, currentUser?.UserName));
            }

            return Unauthorized();
        }
    }
    }