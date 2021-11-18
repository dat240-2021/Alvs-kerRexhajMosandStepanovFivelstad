using backend.Core.Domain.Lobby.Models;
using MediatR;

namespace backend.Core.Domain.Lobby.Events
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