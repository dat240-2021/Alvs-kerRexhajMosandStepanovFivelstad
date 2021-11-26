using System;
using backend.Core.Domain.Games.Events;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public class Proposer : BaseEntity, IProposer
    {
        public Guid Id { get; set; }
        public int Score { get; set; }

        public string Username { get; set; }

        public Proposer(Guid id)
        {
            Id = id;
        }
        public Proposer(Guid id, string username)
        {
            Id = id;
            Username = username;
        }
        public int CalculateScore(TimeSpan RoundTime, TimeSpan timeDelta, int slicesShown, int totalSlices)
        {
            int timeScore = (int)Math.Round(RoundTime.TotalSeconds - timeDelta.TotalSeconds);
            int sliceScore = totalSlices - slicesShown;
            var newScore = timeScore * sliceScore;
            Score += newScore;

            return newScore;
        }

        public void NotifyTurn()
        {
        }

        public string GetId() => Id.ToString();

    }
}