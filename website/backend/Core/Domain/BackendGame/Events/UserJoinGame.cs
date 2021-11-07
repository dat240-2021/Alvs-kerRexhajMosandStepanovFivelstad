using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record UserJoinGame : BaseDomainEvent
    {
        public UserJoinGame(WaitingEntry entry)
        {
            Entry = entry;
        }

        public WaitingEntry Entry { get; }
    }
}