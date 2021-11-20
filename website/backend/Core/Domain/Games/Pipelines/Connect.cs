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
    public class Connect
    {
        public record Request(Guid User): IRequest<Unit> {}

        public class Handler: IRequestHandler<Request, Unit>
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
                Game game = _service.GetByUserId(request.User);

                if (game is not null)
                {
                    game.ConnectUser(request.User);
                }
                else
                {
                    _hub.Clients.User(request.User.ToString()).SendAsync("InvalidGame");
                }

                return Task.FromResult(Unit.Value);
            }
        }
    }
}