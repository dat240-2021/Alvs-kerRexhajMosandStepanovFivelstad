using System;

namespace backend.Core.Domain.GameSpace
{
    public class Guesser
    {
        public Guid Id { get; set; }
        public bool Guessed { get; set; }

        public Guesser(Guid id)
        {
            Id = id;
        }
        public void UpdateScore(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices){
            // if you spend more than 100 seconds guessing on one slice you get no points.
            var timeScore = 120 - timeDelta.TotalSeconds;
            var sliceScore = totalSlices - slicesShown;

            // do something with the score.
            _ = timeScore * sliceScore;
        }
    }
}