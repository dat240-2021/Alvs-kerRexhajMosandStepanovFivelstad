using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.Games.Events
{
    public record GameOverEvent: BaseDomainEvent
    {
        public Guid GameId;
        public List<string> GuesserIds;
        public string ProposerId;
    }

}