using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Pipelines;
using Controllers.Generics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.BackendGame
{
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
            var leaderBoard = await _mediator.Send(new GetLeaderboard.Request());
            return Ok(leaderBoard);
        }
    }
}