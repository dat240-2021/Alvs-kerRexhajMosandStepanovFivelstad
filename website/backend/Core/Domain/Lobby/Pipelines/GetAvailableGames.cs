using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Models;
using backend.Core.Domain.Lobby.Services;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Lobby.Pipelines
{
    public class GetAvailableGames
    {
        public record Request(): IRequest<List<GameWithSlotInfo>> {}

        public class Handler: IRequestHandler<Request, List<GameWithSlotInfo>>
        {

            private readonly GameContext _db;
            private readonly ILobbyService _LobbyService;

            public Handler(GameContext db, ILobbyService LobbyService)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _LobbyService = LobbyService;
            }

            
            public async Task<List<GameWithSlotInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                var games = await _db.Games
                    .Where(g => g.State.Equals(GameState.Created))
                    .ToListAsync(cancellationToken);

                return games.Select(game => new GameWithSlotInfo(game, _LobbyService.GetSlotInfo(game.Id))
                ).ToList();
            }
        }
    }
}

