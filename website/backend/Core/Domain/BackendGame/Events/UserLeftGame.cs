using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record UserLeftGame: BaseDomainEvent
    {
        public UserLeftGame(WaitingEntry entry)
        {
            Entry = entry;
        }

        public WaitingEntry Entry { get; }
    }
}