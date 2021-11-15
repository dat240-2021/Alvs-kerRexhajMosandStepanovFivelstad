using System;
using backend.Core.Domain.BackendGame.Models;
using MediatR;
using SharedKernel;

namespace backend.Core.Domain.BackendGame.Events
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