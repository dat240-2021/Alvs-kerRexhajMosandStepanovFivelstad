using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.Games.Events;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Games.Pipelines
{
    public class StartGame
    {
        public record Request(GameWithSlotInfo Game, List<int> ImageIds): IRequest<Unit> {}
        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly GameContext _db;
            private readonly IGameService _service;
            private readonly IMediator _mediator;

            public Handler(GameContext db, IGameService service, IMediator mediator)
            {
                _db = db;
                _service = service;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                IProposer proposer = new Oracle(request.Game.Game.Id);
                var proposerId = request.Game.SlotInfo.ProposerId;

                if (proposerId is not null)
                {
                    proposer = new Proposer((Guid)proposerId);
                }

                var images = await _db.Images
                    .Where(i => request.ImageIds.Contains(i.Id))
                    .Include(images => images.Slices)
                    .Include(images => images.Label)
                    .ThenInclude(label => label.Category)
                    .ToListAsync(cancellationToken);

                var game = new Game(
                    request.Game.Game.Id,
                    images,
                    request.Game.SlotInfo.GuessersIds.Select(g => new Guesser(g)).ToList(),
                    proposer
                    ) {
                    RoundTime = TimeSpan.FromTicks(request.Game.Game.Settings.Duration)
                };

                await _mediator.Publish(new NewImageEvent()
                {
                    ImageId = game.CurrentImage.Id,
                    GuesserIds = game.Guessers.Select(g => g.Id.ToString()).ToList(),
                    ProposerId = game.Proposer.GetId()
                });

                _service.Add(game);
                return Unit.Value;
            }
        }
    }
}