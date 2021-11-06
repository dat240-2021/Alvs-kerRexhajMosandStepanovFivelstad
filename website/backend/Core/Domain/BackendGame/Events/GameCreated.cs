using System;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record GameCreated : BaseDomainEvent
    {
        public Guid Id { get; }

        public GameCreated(Guid id)
        {
            Id = id;
        }
    }
}