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
    public class UserPropose
    {
        public record Request(Guid User, int SliceNumber) : IRequest<Unit> { }

        public class Handler : IRequestHandler<Request, Unit>
        {
            private IMediator _mediator;
            private IGameService _service;

            public Handler(IMediator mediator, IGameService service)
            {
                _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
                _service = service ?? throw new System.ArgumentNullException(nameof(service));
            }

            async public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = _service.GetByUserId(request.User);

                if (game is not null)
                {
                    await _mediator.Send(new Propose.Request(game.Id, request.SliceNumber));
                }

                return Unit.Value;
            }
        }
    }
}