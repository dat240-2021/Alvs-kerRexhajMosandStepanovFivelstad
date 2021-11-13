using System;
using backend.Core.Domain.BackendGame;
using backend.Core.Domain.BackendGame.Models;

namespace backend.Controllers.BackendGame.Dto
{
    public record GameDto
    {
        public Guid Id { get; protected set; }
        public GameSettings Settings { get; protected set; }
        public GameState State { get; protected set; }
        public int OccupiedSlotsCount { get; set; }
        
        public GameDto(GameWithSlotInfo data)
        {
            Id = data.Game.Id;
            Settings = data.Game.Settings;
            State = data.Game.State;
            OccupiedSlotsCount = data.SlotInfo.PlayerSlots.Count;
        }
    }
}