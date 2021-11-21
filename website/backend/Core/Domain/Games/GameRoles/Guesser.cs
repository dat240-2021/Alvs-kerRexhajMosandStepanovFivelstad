using System;
using backend.Core.Domain.Games.Events;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public class Guesser : BaseEntity
    {
        public Guid Id { get; set; }
        public bool Guessed { get; set; }
        public bool Connected { get; set; } = false;

        public int Score { get; set; } = 0;

        public Guesser(Guid id)
        {
            Id = id;
        }
        public void UpdateScore(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices){


            int timeScore = (int)Math.Round(RoundTime.TotalSeconds - timeDelta.TotalSeconds);
            int sliceScore = totalSlices - slicesShown;

            // do something with the score.
            Score = timeScore * timeScore;

            Events.Add( new PlayerScoredEvent{UserId = Id, Score = Score});
        }

        public string GetId() => Id.ToString();

    }
}