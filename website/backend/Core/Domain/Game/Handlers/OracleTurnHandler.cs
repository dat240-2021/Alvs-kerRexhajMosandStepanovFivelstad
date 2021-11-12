using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using backend.Core.Domain.GameSpace.Events;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.SignalR;
using backend.Hubs;
using backend.Core.Domain.GameSpace.Pipelines;

namespace backend.Core.Domain.GameSpace.Handlers
{
    public class OracleTurnHandler: INotificationHandler<OracleTurnEvent>
    {
        private readonly GameContext _db;
        private readonly IGameService _service;
        private readonly IMediator _mediator;
        private readonly IHubContext<GameHub> _hub;

        public OracleTurnHandler(GameContext db, IGameService service, IMediator mediator, IHubContext<GameHub> hub)
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _service = service ?? throw new System.ArgumentException(nameof(service));
            _mediator = mediator ?? throw new System.ArgumentException(nameof(mediator));
            _hub = hub ?? throw new System.ArgumentException(nameof(hub));
        }

        public async Task Handle(OracleTurnEvent notification, CancellationToken cancellationToken)
        {

            var game = _service.Get(notification.GameId) ?? throw new System.ArgumentException(nameof(IGameService));

            await _mediator.Send(new Propose.Request(game.Id,notification.Proposition));
        }
    }
}