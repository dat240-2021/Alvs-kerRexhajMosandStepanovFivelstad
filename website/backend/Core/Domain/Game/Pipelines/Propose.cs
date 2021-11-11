using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Game.Models;
using Infrastructure.Data;
using MediatR;
using Hubs;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Game.Pipelines
{
    public class Propose
    {
        public record Request(string User, int Proposition): IRequest {}

        public class Handler: IRequestHandler<Request>
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

            
            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                Game game = _service.GetByUserId(request.User);

                if (game is not null)
                {
                    var result = game.Propose(request.Proposition);
                    _hub.Clients.Clients(game.Guessers.Select(g => g.Id)).SendAsync("Tile", result);  
                }
            }
        }
    }
}