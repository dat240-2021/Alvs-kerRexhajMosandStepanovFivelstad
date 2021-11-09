using System;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;

namespace backend.Core.Domain.BackendGame.Services
{
    public interface IBackendGameService
    {
        Task JoinGame(Game game, Guid userId);
        Task LeaveGame(Guid gameId, Guid userId);
        GameSlotInfo GetSlotInfo(Guid gameId);
        bool HasAvailableSlots(Guid gameId);

    }
}