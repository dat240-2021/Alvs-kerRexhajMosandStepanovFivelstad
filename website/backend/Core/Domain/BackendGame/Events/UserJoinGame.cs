using backend.Core.Domain.BackendGame.Models;
using MediatR;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
{
    public record UserJoinGame : INotification
    {
        public UserJoinGame(GameSlotNotification notification)
        {
            Notification = notification;
        }

        public GameSlotNotification Notification { get; }
    }
}