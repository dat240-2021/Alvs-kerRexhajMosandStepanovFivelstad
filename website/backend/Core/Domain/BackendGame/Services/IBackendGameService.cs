using System;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;

namespace backend.Core.Domain.BackendGame.Services
{
    public interface IBackendGameService
    {
        void StoreGame(Game game);
        void JoinGame(Guid gameId, Guid userId, SlotRole role);
        void LeaveGame(Guid gameId, Guid userId);
        GameSlotInfo GetSlotInfo(Guid gameId);
        bool HasAvailableSlots(Guid gameId);
        
        bool DeleteGame(Guid gameId);

    }
}