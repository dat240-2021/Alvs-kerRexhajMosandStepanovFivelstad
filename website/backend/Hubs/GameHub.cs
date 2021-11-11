using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using backend.Core.Domain.Game.Models;
using backend.Core.Domain.Game.Pipelines;
using MediatR;
using SignalR;

namespace backend.Hubs
{
    public class GameHub: Hub
    {
        private readonly IMediator _mediator;

        public GameHub(IMediator mediator) => _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        public async Task Guess(string guess)
        {
            string user = HubCallerContext.UserIdentifier;
            await _mediator.SendAsync(new Request.Guess(User = user, Guess = guess));
        }

        public async Task Propose(int proposition)
        {
            string user = HubCallerContext.UserIdentifier;
            await _mediator.SendAsync(new Request.Propose(User = user, Proposition = proposition));
        }
    }
}
