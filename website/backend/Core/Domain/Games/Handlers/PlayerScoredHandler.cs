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
using Microsoft.Extensions.DependencyInjection;
using backend.Core.Domain.Lobby.Models;

namespace backend.Core.Domain.Games.Handlers
{
    public class PlayerScoredHandler: INotificationHandler<PlayerScoredEvent>
    {
        private readonly IServiceScopeFactory _service;

        private readonly IHubContext<GameHub> _hub;

        public PlayerScoredHandler(IServiceScopeFactory service, IHubContext<GameHub> hub, IMediator mediator)
        {
            _service = service ?? throw new System.ArgumentException(nameof(service));
            _hub = hub ?? throw new System.ArgumentException(nameof(hub));
        }

        public async Task Handle(PlayerScoredEvent notification, CancellationToken cancellationToken)
        {
            using (var scope = _service.CreateScope())
            {
                await _hub.Clients.Users(notification.PlayerIds).SendAsync("APlayerScored", notification.UserName, notification.Score, cancellationToken);


                var db = scope.ServiceProvider.GetRequiredService<GameContext>();

                var dbScore = await db.Scores.Where(x => x.User == notification.UserId).FirstOrDefaultAsync();
                if (dbScore==null){
                    dbScore = new Score(
                        notification.UserId,
                        notification.Score);
                    db.Add(dbScore);
                }

                dbScore.UserScore+= notification.Score;
                await db.SaveChangesAsync();

            }

        }
    }
}