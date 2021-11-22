using System;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Models;

namespace backend.Core.Domain.Lobby.Services
{
    public interface ILobbyService
    {
        void StoreGame(Game game);
        void JoinGame(Guid gameId, Guid userId, SlotRole role);
        void LeaveGame(Guid gameId, Guid userId);
        GameSlotInfo GetSlotInfo(Guid gameId);
        bool HasAvailableSlots(Guid gameId);
        
        bool DeleteGame(Guid gameId);

    }
}