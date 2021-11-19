using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Models;
using MediatR;

namespace backend.Core.Domain.Lobby.Services
{
    public class LobbyService : ILobbyService
    {
        private readonly ConcurrentDictionary<Guid, GameSlotInfo> _games = new ();

        public void StoreGame(Game game)
        {
            if (_games.ContainsKey(game.Id))
            {
                throw new Exception($"Game with id {game.Id} already exists.");
            }
            
            _games.TryAdd(game.Id, new GameSlotInfo
            {
                MaxSlotsCount = game.Settings.ProposerType == "AI" ? game.Settings.GuessersCount : game.Settings.GuessersCount + 1,
                ProposerType = game.Settings.ProposerType == "AI" ? ProposerType.AI : ProposerType.Player
            });
        }
        
        public GameSlotInfo GetSlotInfo(Guid gameId)
        {
            if (_games.TryGetValue(gameId, out var gameSlotInfo))
            {
                return gameSlotInfo;
            }

            throw new Exception($"Game room with id { gameId } is not stored");
        }
        
        public bool HasAvailableSlots(Guid gameId)
        {
            return _games.TryGetValue(gameId, out var gameSlotInfo) ? 
                gameSlotInfo.PlayerSlots.Count < gameSlotInfo.MaxSlotsCount :
                throw new Exception($"Game with id {gameId} not found");
        }

        public bool DeleteGame(Guid gameId)
        {
            return _games.TryRemove(gameId, out _);
        }

        public void JoinGame(Guid gameId, Guid userId, SlotRole role)
        {
            if (!_games.ContainsKey(gameId))
            {
                throw new Exception($"Game room with id {gameId} is not stored");
            }

            if (!_games.TryGetValue(gameId, out var gameSlotInfo)) return;

            if (gameSlotInfo.ProposerType.Equals(ProposerType.AI) && role.Equals(SlotRole.Proposer))
            {
                throw new Exception($"Trying to add proposer player in a game with configured proposer type AI and id { gameId }");
            }

            if (role.Equals(SlotRole.Proposer) && gameSlotInfo.ProposerId is not null)
            {
                throw new Exception($"A proposer has already joing the game with id { gameId }");
            }

            if (gameSlotInfo.MaxSlotsCount.Equals(gameSlotInfo.PlayerSlots.Count))
            {
                throw new Exception($"Room with id { gameId } is full");
            }

            if (gameSlotInfo.PlayerIds.Contains(userId))
            {
                throw new Exception($"User with id { userId } is already in room with id { gameId }");
            }

            var slot = new Slot(userId, role);
            gameSlotInfo.PlayerSlots.Add(slot);
        }

        public void LeaveGame(Guid gameId, Guid userId)
        {
            if (!_games.ContainsKey(gameId))
            {
                throw new Exception($"Game with id {gameId} not found");
            }

            if (!_games.TryGetValue(gameId, out var gameSlotInfo)) return;
            
            if (!gameSlotInfo.PlayerIds.Contains(userId))
            {
                throw new Exception($"User with id { userId } is not found in room { gameId }");
            }
                
            var slot = gameSlotInfo.PlayerSlots.Find(s => s.Id.Equals(userId));
            gameSlotInfo.PlayerSlots.Remove(slot);
        }
    }
}