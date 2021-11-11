using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using backend.Core.Domain.GameSpace.Pipelines;
using MediatR;
using System;

namespace backend.Hubs
{
    public class GameHub: Hub
    {
        private readonly IMediator _mediator;

        public GameHub(IMediator mediator) => _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        public async Task Guess(string guess)
        {
            string user = Context.UserIdentifier;
            await _mediator.Send(new Guess.Request(Guid.Parse(user), guess));
        }

        public async Task Propose(int proposition)
        {
            string user = Context.UserIdentifier;
            await _mediator.Send(new Propose.Request(Guid.Parse(user), proposition));
        }
    }
}
