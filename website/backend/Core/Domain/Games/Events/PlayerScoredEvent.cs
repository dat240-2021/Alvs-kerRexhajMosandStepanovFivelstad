using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.Games.Events
{
    public record PlayerScoredEvent: BaseDomainEvent
    {
        public Guid UserId;
        public int Score;
    }
}