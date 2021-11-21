using System;
using backend.Core.Domain.Games.Events;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public class Proposer : BaseEntity, IProposer
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public Proposer(Guid id)
        {
            Id = id;
        }
        public Proposer(Guid id,string uname)
        {
            Id = id;
            UserName = uname;
        }
            public int ScoreCalc(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices, int nGuessers){

            int timeScore = (int)Math.Round(RoundTime.TotalSeconds - timeDelta.TotalSeconds);
            int sliceScore = totalSlices - slicesShown;

            return timeScore * sliceScore;
        }

        public void NotifyTurn(){
        }

        public string GetId() => Id.ToString();
        public string GetUsername() => UserName;

    }
}