using System;
using SharedKernel;
using backend.Core.Domain.Images;

namespace backend.Core.Domain.Games.Events
{
    public record CorrectGuessEvent: BaseDomainEvent
    {
        public CorrectGuessEvent(Game game, Guid userId, string guess, bool hasMoreRounds, bool isVersusOracle, Image image)
        {
            Game = game;
            UserId = userId;
            Guess = guess;
            HasMoreRounds = hasMoreRounds;
            Image = image;
            WillAutoContinue = hasMoreRounds && isVersusOracle;
        }
        
        public Game Game { get; set; }
        public Guid UserId { get; set; }
        public string Guess { get; set; }
        
        public bool HasMoreRounds { get; set; }
        
        public bool WillAutoContinue { get; set; }
        
        public Image Image { get; set; }
    }
}