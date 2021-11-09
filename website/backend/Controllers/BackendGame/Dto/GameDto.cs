using System;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Pipelines;

namespace backend.Controllers.BackendGame.Dto
{
    public record GameDto
    {
        public Guid Id { get; protected set; }
        public GameSettings Settings { get; protected set; }
        public GameState State { get; protected set; }
        public int OccupiedSlotsCount { get; set; } = 0;

        public GameDto(Game game)
        {
            Id = game.Id;
            Settings = game.Settings;
            State = game.State;
        }
        public GameDto(GameWithSlotInfo data)
        {
            Id = data.Game.Id;
            Settings = data.Game.Settings;
            State = data.Game.State;
            OccupiedSlotsCount = data.SlotInfo.Players.Count;
        }
    }
}