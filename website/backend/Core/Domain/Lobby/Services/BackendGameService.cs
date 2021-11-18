using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Models;
using MediatR;

namespace backend.Core.Domain.Lobby.Services
{
    public class BackendGameService : IBackendGameService
    {
        private readonly IMediator _mediator;
        private readonly ConcurrentDictionary<Guid, GameSlotInfo> _games = new ();

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

            var slotInfo = new GameSlotInfo {MaxSlotsCount = game.Settings.GuessersCount};
            _games.TryAdd(game.Id, slotInfo);
            return slotInfo;
        }
        
        public bool HasAvailableSlots(Guid gameId)
        {
            return _games.TryGetValue(gameId, out var gameSlotInfo) ? 
                gameSlotInfo.PlayerSlots.Count < gameSlotInfo.MaxSlotsCount :
                throw new Exception($"Game with id {gameId} not found");
        }

        public bool deleteGame(Guid gameId)
        {
            return _games.TryRemove(gameId, out _);
        }

        public async Task JoinGame(Game game, Guid userId, SlotRole role)
        {
            if (!_games.ContainsKey(game.Id))
            {
                _games.TryAdd(game.Id, new GameSlotInfo{ MaxSlotsCount = game.Settings.GuessersCount });
            }

            if (_games.TryGetValue(game.Id, out var gameSlotInfo))
            {
                if (gameSlotInfo.MaxSlotsCount.Equals(gameSlotInfo.PlayerSlots.Count))
                {
                    throw new Exception($"Room with id { game.Id } is full");
                }

                if (gameSlotInfo.PlayerIds.Contains(userId))
                {
                    throw new Exception($"User with id { userId } is already in room with id { game.Id }");
                }

                var slot = new Slot(userId, role);
                gameSlotInfo.PlayerSlots.Add(slot);
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
                if (!gameSlotInfo.PlayerIds.Contains(userId))
                {
                    throw new Exception($"User with id { userId } is not found in room { gameId }");
                }
                
                var slot = gameSlotInfo.PlayerSlots.Find(s => s.Id.Equals(userId));
                gameSlotInfo.PlayerSlots.Remove(slot);
            }

            
        }
    }
}