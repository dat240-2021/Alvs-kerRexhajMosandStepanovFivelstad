using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Games.Events;
using backend.Core.Domain.Lobby.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace backend.Core.Domain.Games.Pipelines
{
    public class StartGame
    {
        public record Request(GameWithSlotInfo Game, List<int> ImageIds) : IRequest<Unit> { }
        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly GameContext _db;
            private readonly IGameService _service;

            public Handler(GameContext db, IGameService service)
            {
                _db = db;
                _service = service;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                IProposer proposer = new Oracle(request.Game.Game.Id);
                var proposerId = request.Game.SlotInfo.ProposerId;

                if (proposerId is not null)
                {
                    proposer = new Proposer(
                        (Guid)proposerId,
                        _db.Users.Where(u => u.Id == proposerId).FirstOrDefault().UserName
                        );
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
                    request.Game.SlotInfo.GuessersIds.Select(g => new Guesser(
                        g,
                        _db.Users.Where(u => u.Id == g).FirstOrDefault().UserName
                    )).ToList(),
                    proposer
                    )
                {
                    RoundTime = TimeSpan.FromSeconds(request.Game.Game.Settings.Duration)
                };

                _service.Add(game);
                return Unit.Value;
            }
        }
    }
}