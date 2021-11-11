using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using backend.Hubs;

namespace backend.Core.Domain.GameSpace.Pipelines
{
    public class Propose
    {
        public record Request(Guid User, int SliceNumber): IRequest<Unit> {}

        public class Handler: IRequestHandler<Request,Unit>
        {

            private GameContext _db;
            private IGameService _service;
            private IHubContext<GameHub> _hub;
            public Handler(GameContext db, IGameService service, IHubContext<GameHub> hub)
            {
                _db = db ?? throw new System.ArgumentNullException(nameof(db));
                _service = service ?? throw new System.ArgumentNullException(nameof(service));
                _hub = hub ?? throw new System.ArgumentNullException(nameof(hub));
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                Game game = _service.GetByUserId(request.User);

                if (game is not null)
                {
                    var result = game.Propose(new ProposeDto() { ProposerId = request.User, SliceNumber = request.SliceNumber });
                    if (result is not null)
                    {
                        _hub.Clients.Clients(game.Guessers.Select(g => g.Id.ToString())).SendAsync("Tile", result); 
                    }
                }

                return Task.FromResult(Unit.Value);
            }
        }
    }
}