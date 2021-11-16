using System;
using backend.Core.Domain.Games.Events;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public class Guesser : BaseEntity
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


        public string GetId() => Id.ToString();

    }
}