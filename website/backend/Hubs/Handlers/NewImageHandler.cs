using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Events;
using MediatR;
using System;
using Microsoft.AspNetCore.SignalR;
using Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace backend.Hubs.Handlers
{
    public class NewImageHandler: INotificationHandler<NewImageEvent>
    {
        private readonly IHubContext<GameHub> _hub;
        private readonly GameContext _db;

        public NewImageHandler(IHubContext<GameHub> hub, GameContext db)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Handle(NewImageEvent notification, CancellationToken cancellationToken)
        {
            var image = await _db.Images.Where(x => x.Id == notification.ImageId).SingleOrDefaultAsync();

            if (notification.ProposerId is not null)
            {
                await _hub.Clients.User(notification.ProposerId).SendAsync("NewImageProposer", image, cancellationToken);
            }

            await _hub.Clients.Users(notification.GuesserIds).SendAsync("NewImageGuesser", cancellationToken);
        }
    }
}