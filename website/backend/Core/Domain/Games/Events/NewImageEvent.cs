using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.Games.Events
{
    public record NewImageEvent: BaseDomainEvent
    {
        public int ImageId;
        public List<string> GuesserIds;
        public string ProposerId;

    }

}