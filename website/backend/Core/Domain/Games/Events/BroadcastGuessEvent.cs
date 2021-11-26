using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.Games.Events
{
    public record BroadcastGuessEvent : BaseDomainEvent
    {
        public List<string> PlayerIds;
        public string Guess;
        public string Username;
    }

}