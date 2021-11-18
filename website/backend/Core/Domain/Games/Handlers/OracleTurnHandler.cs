using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using backend.Core.Domain.Games.Events;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.SignalR;
using backend.Hubs;
using backend.Core.Domain.Games.Pipelines;

namespace backend.Core.Domain.Games.Handlers
{
    public class OracleTurnHandler: INotificationHandler<OracleTurnEvent>
    {
        private readonly IGameService _service;
        private readonly IMediator _mediator;

        public OracleTurnHandler(IGameService service, IMediator mediator)
        {
            _service = service ?? throw new System.ArgumentException(nameof(service));
            _mediator = mediator ?? throw new System.ArgumentException(nameof(mediator));
        }

        public async Task Handle(OracleTurnEvent notification, CancellationToken cancellationToken)
        {

            var game = _service.Get(notification.GameId) ?? throw new System.ArgumentException(nameof(IGameService));

            await _mediator.Send(new Propose.Request(game.Id, notification.Proposition));
        }
    }
}