using backend.Core.Domain.BackendGame.Models;
using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.Games.Events
{
    public record OracleTurnEvent : BaseDomainEvent
    {
        public Guid GameId;
        public int Proposition;

    }

}