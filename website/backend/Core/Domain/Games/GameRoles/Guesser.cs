using System;
using backend.Core.Domain.Games.Events;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public class Guesser : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public bool Guessed { get; set; }
        public bool Connected { get; set; } = false;

        public Guesser(Guid id, string uname)
        {
            Id = id;
            UserName = uname;
        }
        public int ScoreCalc(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices){


            int timeScore = (int)Math.Round(RoundTime.TotalSeconds - timeDelta.TotalSeconds);
            int sliceScore = totalSlices - slicesShown;

            return timeScore * sliceScore;

        }

        public string GetId() => Id.ToString();

    }
}