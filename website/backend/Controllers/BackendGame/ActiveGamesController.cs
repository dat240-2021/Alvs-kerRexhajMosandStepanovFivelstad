using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.BackendGame
{
    [ApiController]
    [Route("/api/games")]
    public class ActiveGamesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActiveGamesController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return null;
        }
    }
}