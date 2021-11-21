using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.Games.Events
{
    public record PlayerScoredEvent: BaseDomainEvent
    {
        public List<String> PlayerIds;
        public Guid UserId;
        public string UserName;
        public int Score;
    }
}