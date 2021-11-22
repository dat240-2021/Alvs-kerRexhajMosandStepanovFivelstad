using System;
using SharedKernel;

namespace backend.Core.Domain.Games.Events
{
    public record FullyVisibleImageWithoutCorrectGuessesEvent: BaseDomainEvent
    {
        public Guid GameId;
        public string Guess;
    }
}