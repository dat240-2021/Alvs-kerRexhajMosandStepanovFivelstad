using System;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Models;

namespace backend.Core.Domain.Lobby.Services
{
    public interface IBackendGameService
    {
        Task JoinGame(Game game, Guid userId, SlotRole role);
        Task LeaveGame(Guid gameId, Guid userId);
        GameSlotInfo GetSlotInfo(Game game);
        bool HasAvailableSlots(Guid gameId);
        
        bool deleteGame(Guid gameId);

    }
}