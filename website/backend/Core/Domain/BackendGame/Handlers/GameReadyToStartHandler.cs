using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Events;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Handlers
{
    public class GameReadyToStartHandler: INotificationHandler<GameReadyToStart>
    {
        private readonly GameContext _db;
        private readonly IMediator _mediator;

        public GameReadyToStartHandler(GameContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentException(nameof(db));
            _mediator = mediator;
        }
        
        public async Task Handle(GameReadyToStart notification, CancellationToken cancellationToken)
        {
            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id.Equals(notification.Game.Id), cancellationToken) ??
                       throw new Exception($"Game with id {notification.Game.Id} not found");
            
            if (notification.Game.Settings.ProposerType == "AI")
            {
                await _mediator.Send(new StartGame(game), cancellationToken);
            }
            
            
            
            throw new NotImplementedException();
        }
    }
}