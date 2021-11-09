using backend.Core.Domain.BackendGame.Models;
using MediatR;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record UserLeftGame: INotification
    {
        public UserLeftGame(GameSlotNotification notification)
        {
            Notification = notification;
        }

        public GameSlotNotification Notification { get; }
    }
}