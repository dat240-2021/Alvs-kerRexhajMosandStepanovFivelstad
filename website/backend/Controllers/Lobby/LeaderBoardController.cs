using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Pipelines;
using Controllers.Generics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Lobby
{
    [Authorize]
    [ApiController]
    [Route("/api/leaderboard")]
    public class LeaderBoardController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public LeaderBoardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetLeaderboard.Request()) );
        }
    }
}