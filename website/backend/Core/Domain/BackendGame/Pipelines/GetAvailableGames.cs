using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Services;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.BackendGame.Pipelines
{
    public class GetAvailableGames
    {
        public record Request(): IRequest<List<GameWithSlotInfo>> {}

        public class Handler: IRequestHandler<Request, List<GameWithSlotInfo>>
        {

            private readonly GameContext _db;
            private readonly IBackendGameService _backendGameService;

            public Handler(GameContext db, IBackendGameService backendGameService)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _backendGameService = backendGameService;
            }

            
            public async Task<List<GameWithSlotInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                var statesToInclude = new List<GameState>(){ GameState.Active, GameState.Created }; 
                
                var games = await _db.Games
                    .Where(g => statesToInclude.Contains(g.State))
                    .ToListAsync(cancellationToken);

                return games.Select(game => new GameWithSlotInfo(game, _backendGameService.GetSlotInfo(game))
                ).ToList();
            }
        }
    }
}

