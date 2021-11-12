using System;
using backend.Core.Domain.GameSpace.Events;
using SharedKernel;

namespace backend.Core.Domain.GameSpace
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


        public void MyTurn(){
            Events.Add( new ProposersTurnEvent(){ProposerId = Id.ToString()});
        }

        public string GetId() => Id.ToString();
    }
}