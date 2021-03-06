using System;
using System.Collections.Generic;
using System.Linq;
using backend.Core.Domain.Games.Events;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public class Oracle : BaseEntity, IProposer
    {

        public Guid GameId;

        public int Score => 0;
        public string Username => "";
        private Queue<int> _proposals;
        public int Proposal
        {
            get
            {
                return _proposals.Dequeue();
            }
        }
        // never used
        // private int _index;
        public Oracle(Guid id)
        {
            GameId = id;
        }

        public int CalculateScore(TimeSpan RoundTime, TimeSpan timeDelta, int slicesShown, int totalSlices) => -1;

        public void NotifyTurn()
        {
        }

        public string GetId() => null;
        public string GetUsername() => "";

        public void HandleNewImage(List<int> slices)
        {
            var random = new Random();

            for (int i = 0; i < slices.Count; i++)
            {
                var j = random.Next(0, slices.Count);
                var temp = slices[j];
                slices[j] = slices[i];
                slices[i] = temp;
            }

            _proposals = new Queue<int>(slices);
        }
    }

}
