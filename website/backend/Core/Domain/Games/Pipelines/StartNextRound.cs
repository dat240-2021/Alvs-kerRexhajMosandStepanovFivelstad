using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace backend.Core.Domain.Games.Pipelines
{
    public class StartNextRound
    {
        public record Request(Guid User): IRequest<Unit> {}

        public class Handler: IRequestHandler<Request, Unit>
        {

            private IMediator _mediator;
            private IGameService _service;
            public Handler(IMediator mediator, IGameService service, IHubContext<GameHub> hub)
            {
                _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
                _service = service ?? throw new System.ArgumentNullException(nameof(service));
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = _service.GetByUserId(request.User) ?? throw new ArgumentException(nameof(IGameService));
                game.StartNextRound();
                return Task.FromResult(Unit.Value);
            }
        }
    }
}