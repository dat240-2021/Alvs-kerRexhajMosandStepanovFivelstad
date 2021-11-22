using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using backend.Core.Domain.Games.Events;
using backend.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Games.Pipelines
{
    public class Guess
    {
        public record Request(Guid User, string Guess): IRequest<Unit> {}

        public class Handler: IRequestHandler<Request,Unit>
        {
            
            private readonly IGameService _service;
            private readonly IHubContext<GameHub> _hub;
            
            public Handler(IGameService service, IHubContext<GameHub> hub)
            {
                _service = service ?? throw new ArgumentNullException(nameof(service));
                _hub = hub;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = _service.GetByUserId(request.User);

                if (game is null) return Unit.Value;
                
                var result = game.Guess(new GuessDto(){ User = request.User, Guess = request.Guess });


                if (!result) return Unit.Value;
                
                await _hub.Clients.Users(game.PlayerIds).SendAsync("Guess", request, cancellationToken);

                return Unit.Value;
            }
        }
    }
}