using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.Games.Events
{
    public record GuessersTurnEvent: BaseDomainEvent
    {
        public List<string> PlayerIds;
    }
}