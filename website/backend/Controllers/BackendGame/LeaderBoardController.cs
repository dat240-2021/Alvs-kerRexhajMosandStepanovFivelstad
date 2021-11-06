using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.BackendGame
{
    [ApiController]
    [Route("/api/leaderboard")]
    public class LeaderBoardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaderBoardController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var leaderBoard = await _mediator.Send(new GetLeaderboard.Request());
            return Ok(leaderBoard);
        }
    }
}