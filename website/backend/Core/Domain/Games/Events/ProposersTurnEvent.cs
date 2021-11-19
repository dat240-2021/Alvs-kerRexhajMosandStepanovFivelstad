using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.Games.Events
{
    public record ProposersTurnEvent: BaseDomainEvent
    {
        public List<string> PlayerIds;
    }
}