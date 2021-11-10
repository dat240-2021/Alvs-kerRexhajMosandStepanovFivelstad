using System.Threading.Tasks;
using Controllers.Authentication;
using Controllers.Authentication.Dto;
using Controllers.Generics;
using Domain.Authentication.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // using Microsoft.AspNetCore.Http.StatusCode;

namespace backend.Controllers.Authentication
{

    [Authorize]
    [ApiController]
    // [Route("[controller]")]
    [Route("/api/logout")]
    public class LogoutController : ApiBaseController
    {
        private readonly ILogger<LogoutController> _logger;
		private readonly IMediator _mediator;

        public LogoutController(ILogger<LogoutController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDto user){
            await _mediator.Send(new LogoutUser.Request());
            return Ok();

        }
    }
    }