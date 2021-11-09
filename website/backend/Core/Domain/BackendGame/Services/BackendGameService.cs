using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;
using MediatR;

namespace backend.Core.Domain.BackendGame.Services
{
    public class BackendGameService : IBackendGameService
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<Guid, GameSlotInfo> _games = new ();

        public BackendGameService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public GameSlotInfo GetSlotInfo(Game game)
        {
            if (_games.TryGetValue(game.Id, out var gameSlotInfo))
            {
                return gameSlotInfo;
            }

            var slotInfo = new GameSlotInfo {MaxSlotsCount = game.Settings.PlayersCount};
            _games.Add(game.Id, slotInfo);
            return slotInfo;
        }
        
        public bool HasAvailableSlots(Guid gameId)
        {
            return _games.TryGetValue(gameId, out var gameSlotInfo) ? 
                gameSlotInfo.Players.Count < gameSlotInfo.MaxSlotsCount :
                throw new Exception($"Game with id {gameId} not found");
        }

        public async Task JoinGame(Game game, Guid userId)
        {
            if (!_games.ContainsKey(game.Id))
            {
                
                _games.Add(game.Id, new GameSlotInfo{ MaxSlotsCount = game.Settings.PlayersCount });
            }

            if (_games.TryGetValue(game.Id, out var gameSlotInfo))
            {
                if (gameSlotInfo.MaxSlotsCount.Equals(gameSlotInfo.Players.Count))
                {
                    throw new Exception($"Room with id { game.Id } is full");
                }

                if (gameSlotInfo.Players.Contains(userId))
                {
                    throw new Exception($"User with id { userId } is already in room with id { game.Id }");
                }
                
                gameSlotInfo.Players.Add(userId);
            }
        }

        public async Task LeaveGame(Guid gameId, Guid userId)
        {
            if (!_games.ContainsKey(gameId))
            {
                throw new Exception($"Game with id {gameId} not found");
            }
            
            if (_games.TryGetValue(gameId, out var gameSlotInfo))
            {
                if (!gameSlotInfo.Players.Contains(userId))
                {
                    throw new Exception($"User with id { userId } is not found in room { gameId }");
                }
                
                gameSlotInfo.Players.Remove(userId);
            }

            
        }
    }
}