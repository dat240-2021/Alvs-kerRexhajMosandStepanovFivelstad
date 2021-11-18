using System;
using backend.Core.Domain.Games.Events;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public class Proposer : BaseEntity, IProposer
    {
        public Guid Id { get; set; }

        public Proposer(Guid id)
        {
            Id = id;
        }

            public void UpdateScore(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices, int nGuessers){

            int timeScore = (int)Math.Round(RoundTime.TotalSeconds - timeDelta.TotalSeconds);
            int sliceScore = totalSlices - slicesShown;

            // do something with the score.
            int Score = timeScore * timeScore;
        }

        public void NotifyTurn(){
        }

        public string GetId() => Id.ToString();
    }
}