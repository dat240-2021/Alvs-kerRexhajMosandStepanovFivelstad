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

namespace backend.Core.Domain.GameSpace.Handlers
{
    public class GameOverHandler: INotificationHandler<GameOverEvent>
    {
        private readonly GameContext _db;
        private readonly IGameService _service;
        private IHubContext<GameHub> _hub;


        public GameOverHandler(GameContext db, IGameService service,IHubContext<GameHub> hub )
        {
            _db = db ?? throw new System.ArgumentException(nameof(db));
            _service = service ?? throw new System.ArgumentException(nameof(service));
            _hub = hub ?? throw new System.ArgumentException(nameof(hub));
        }

        public async Task Handle(GameOverEvent notification, CancellationToken cancellationToken)
        {
            _service.Remove(notification.GameId);
            await _hub.Clients.Clients(notification.GuesserIds.Select(g => g.ToString())).SendAsync("GameOver", cancellationToken);
            await _hub.Clients.Clients(notification.ProposerId.Select(g => g.ToString())).SendAsync("GameOver", cancellationToken);
        }
    }
}