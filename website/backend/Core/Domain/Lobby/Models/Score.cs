using System;
using Domain.Authentication;

namespace backend.Core.Domain.Lobby.Models
{
    public class Score
    {
        public int Id { get; set; }
        public User User { get; set; }
        public double UserScore { get; set; }
        public DateTime Date { get; set; }
    }
}