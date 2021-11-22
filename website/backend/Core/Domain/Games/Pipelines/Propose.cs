using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using backend.Hubs;

namespace backend.Core.Domain.Games.Pipelines
{
    public class Propose
    {
        public record Request(Guid GameId, int SliceNumber): IRequest<Unit> {}

        public class Handler: IRequestHandler<Request,Unit>
        {

            private IGameService _service;
            private IHubContext<GameHub> _hub;
            public Handler(IGameService service, IHubContext<GameHub> hub)
            {
                _service = service ?? throw new System.ArgumentNullException(nameof(service));
                _hub = hub ?? throw new System.ArgumentNullException(nameof(hub));
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {

            var game =  _service.Get(request.GameId);

            var result = game.Propose(request.SliceNumber);
            if (result is not null)
            {
                _hub.Clients.Users(game.GuesserIds).SendAsync("Proposal", result, cancellationToken);
            }
                return Task.FromResult(Unit.Value);
            }
        }
    }
}