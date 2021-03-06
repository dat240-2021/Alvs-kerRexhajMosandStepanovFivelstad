using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Images.Pipelines;
using backend.Core.Domain.Lobby.Events;
using backend.Core.Domain.Lobby.Models;
using backend.Core.Domain.Lobby.Services;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Lobby.Pipelines
{
    public class StartGame
    {
        public record Request(Guid GameId, Guid? UserId = null) : IRequest<Response> { }

        public record Response(bool Success);

        public class Handler : IRequestHandler<Request, Response>
        {

            private readonly GameContext _db;
            private readonly IMediator _mediator;
            private readonly ILobbyService _LobbyService;

            public Handler(GameContext db, IMediator mediator, ILobbyService LobbyService)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
                _LobbyService = LobbyService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = await _db.Games.Include(g => g.Creator).FirstOrDefaultAsync(g => g.Id.Equals(request.GameId), cancellationToken);
                if (request.UserId is not null && request.UserId != game.Creator.Id)
                {
                    return new Response(false);
                }
                game.State = GameState.Active;
                await _db.SaveChangesAsync(cancellationToken);

                var imageIds = await _mediator.Send(new GetImageIdsListByCategoriesIds.Request(game.Settings.CategoryIds, game.Settings.ImagesCount, (Guid)request.UserId));

                var slotInfo = _LobbyService.GetSlotInfo(game.Id);

                await _mediator.Send(new Games.Pipelines.StartGame.Request(new GameWithSlotInfo(game, slotInfo), imageIds), cancellationToken);
                await _mediator.Publish(new GameStarted(game), cancellationToken);
                return new Response(true);
            }
        }
    }
}