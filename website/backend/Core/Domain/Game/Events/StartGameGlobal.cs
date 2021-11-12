using backend.Core.Domain.BackendGame.Models;
using System;
using SharedKernel;
using System.Collections.Generic;

namespace backend.Core.Domain.GameSpace.Events
{
    public record StartGame: BaseDomainEvent
    {
        public int GameId { get; protected set; }
        public Guid? ProposerId { get; protected set; }
        public List<Guid> GuesserIds { get; protected set; }
        public List<int> ImageIds { get; protected set; }
        public TimeSpan RoundTime { get; protected set; }
    }
}