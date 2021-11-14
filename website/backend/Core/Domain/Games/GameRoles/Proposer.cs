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

            public void UpdateScore(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices){
            // if you spend more than 100 seconds guessing on one slice you get no points.
            var timeScore = 120 - timeDelta.TotalSeconds;
            var sliceScore = totalSlices - slicesShown;

            // do something with the score.
            _ = timeScore * sliceScore;
        }


        public void NotifyTurn(){
            Events.Add( new ProposersTurnEvent(){ProposerId = Id.ToString()});
        }

        public string GetId() => Id.ToString();
    }
}