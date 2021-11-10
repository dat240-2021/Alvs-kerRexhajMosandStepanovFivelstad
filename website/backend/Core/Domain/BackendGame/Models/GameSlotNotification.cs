using System;

namespace backend.Core.Domain.BackendGame.Models
{
    public class GameSlotNotification
    {
        public GameSlotNotification(Guid gameId, int occupiedSlotsCount)
        {
            GameId = gameId;
            OccupiedSlotsCount = occupiedSlotsCount;
        }

        public Guid GameId { get; set; }
        
        public int OccupiedSlotsCount { get; set; }
    }
}