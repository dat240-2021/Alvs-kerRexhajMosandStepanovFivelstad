using System;
using backend.Core.Domain.BackendGame.Events;
using backend.Core.Domain.BackendGame.Pipelines;
using Domain.Authentication;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace backend.Core.Domain.BackendGame
{
    public class WaitingEntry : BaseEntity
    {
        public int Id { get; set; }
        public User User { get; protected set; }
        public Guid GameId { get; protected set; }
        
        public WaitingEntry() { }
        
        public WaitingEntry(User user, Guid gameId)
        {
            User = user;
            GameId = gameId;
            Events.Add(new UserJoinGame(this));
        }
        
    }
}