using backend.Core.Domain.BackendGame.Models;
using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.GameSpace.Events
{
    public record ProposersTurnEvent: BaseDomainEvent
    {
        public string ProposerId;
    }
}