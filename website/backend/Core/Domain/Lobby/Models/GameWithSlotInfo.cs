namespace backend.Core.Domain.Lobby.Models
{
    public class GameWithSlotInfo
    {
        public Game Game { get; set; }
        public GameSlotInfo SlotInfo { get; set; }

        public GameWithSlotInfo(Game game, GameSlotInfo slotInfo)
        {
            Game = game;
            SlotInfo = slotInfo;
        }
    }
}