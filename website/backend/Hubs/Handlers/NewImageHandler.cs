using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Events;
using backend.Core.Domain.Images.Pipelines;
using MediatR;
using System;
using Microsoft.AspNetCore.SignalR;
using Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Hubs.Handlers
{
    public class NewImageHandler: INotificationHandler<NewImageEvent>
    {
        private readonly IHubContext<GameHub> _hub;
        private IServiceScopeFactory _scopeFactory;

        public NewImageHandler(IHubContext<GameHub> hub, IServiceScopeFactory scopeFactory)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        public async Task Handle(NewImageEvent notification, CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<GameContext>();
                var image = await db.Images
                    .Where(image => image.Id == notification.ImageId)
                    .Include(image => image.Slices)
                    .Include(image => image.Label)
                    .ThenInclude(label => label.Category)
                    .FirstOrDefaultAsync();

                if (notification.ProposerId is not null)
                {
                    await _hub.Clients.User(notification.ProposerId).SendAsync("NewImageProposer", image, cancellationToken);
                }

                await _hub.Clients.Users(notification.GuesserIds).SendAsync("NewImageGuesser", cancellationToken);
            }
        }
    }
}