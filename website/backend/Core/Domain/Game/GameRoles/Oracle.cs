using System;
using System.Collections.Generic;
using System.Linq;
using backend.Core.Domain.GameSpace.Events;
using SharedKernel;

namespace backend.Core.Domain.GameSpace
{
    public class Oracle : BaseEntity, IProposer
    {

        public Guid GameId;
        private List<int> _propositions;
        private int _index;
        public Oracle(Guid id) {
            GameId = id;
        }

        public void UpdateScore(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices){}

        public void NotifyTurn(){
            Events.Add( new OracleTurnEvent(){ GameId = GameId, Proposition = _propositions[_index++]});
        }

        public string? GetId() => null;

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

            _propositions = slices;
        }
    }

}
