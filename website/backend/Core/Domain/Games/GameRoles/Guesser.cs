using System;
using backend.Core.Domain.Games.Events;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public class Guesser : BaseEntity
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public string Username { get; set; }
        public bool Guessed { get; set; }
        public bool Connected { get; set; } = false;

        public Guesser(Guid id, string username)
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

        public string GetId() => Id.ToString();

    }
}