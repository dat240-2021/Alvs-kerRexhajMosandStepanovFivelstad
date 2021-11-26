using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Pipelines;
using MediatR;
using System;

namespace backend.Hubs
{
    public class GameHub : Hub
    {
        private readonly IMediator _mediator;

        public GameHub(IMediator mediator) => _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));

        public override async Task OnDisconnectedAsync(Exception e)
        {
            string user = Context.UserIdentifier;
            await _mediator.Send(new Disconnect.Request(Guid.Parse(user)));
            await base.OnDisconnectedAsync(e);
        }

        public async Task Disconnect()
        {
            string user = Context.UserIdentifier;
            await _mediator.Send(new Disconnect.Request(Guid.Parse(user)));
        }

        public async Task Connect()
        {
            string user = Context.UserIdentifier;
            await _mediator.Send(new Connect.Request(Guid.Parse(user)));
        }
        public async Task Guess(string guess)
        {
            string user = Context.UserIdentifier;
            await _mediator.Send(new Guess.Request(Guid.Parse(user), guess));
        }

        public async Task Propose(int proposition)
        {
            string user = Context.UserIdentifier;
            await _mediator.Send(new UserPropose.Request(Guid.Parse(user), proposition));
        }
    }
}
