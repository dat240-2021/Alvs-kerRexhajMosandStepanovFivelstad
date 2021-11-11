using backend.Core.Domain.BackendGame.Models;
using System;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record StartGame: BaseDomainEvent
    {
        public int GameId { get; protected set; }
        public Guid ProposerId { get; protected set; }
        public List<Guid> GuesserIds { get; protected set; }
        public List<int> ImageIds { get; protected set; }
        public TimeSpan RoundTime { get; protected set; }
    }
}