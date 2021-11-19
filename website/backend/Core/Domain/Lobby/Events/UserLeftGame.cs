using System;
using backend.Core.Domain.Lobby.Models;
using MediatR;

namespace backend.Core.Domain.Lobby.Events
{
    public record UserLeftGame: INotification
    {
        public UserLeftGame(GameSlotNotification notification, Guid userId)
        {
            Notification = notification;
            UserId = userId;
        }

        public GameSlotNotification Notification { get; }
        public Guid UserId { get; }
    }
}