using System;
using backend.Core.Domain.Images;

namespace backend.Core.Domain.Games.Dtos
{
    public class CorrectGuessDto
    {
        public int NewGuesserScore { get; set; }
        public int NewProposerScore { get; set; }
        public string Guesser { get; set; }
        public Image Image { get; set; }
        public string Guess { get; set; }
        public bool HasMoreRounds { get; set; }
        public bool WillAutoContinue { get; set; }
    }
}