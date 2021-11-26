using System;
using SharedKernel;
using backend.Core.Domain.Images;

namespace backend.Core.Domain.Games.Events
{
    public record CorrectGuessEvent : BaseDomainEvent
    {
        public string[] PlayerIds { get; set; }
        public Guesser Guesser { get; set; }
        public int GuesserScored { get; set; }
        public IProposer Proposer { get; set; }
        public int ProposerScored { get; set; }
        public string Guess { get; set; }
        public bool HasMoreRounds { get; set; }
        public bool WillAutoContinue { get; set; }

        public Image Image { get; set; }
    }
}