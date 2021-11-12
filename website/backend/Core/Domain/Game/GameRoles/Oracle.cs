using System;
using System.Collections.Generic;
using System.Linq;
using SharedKernel;

namespace backend.Core.Domain.GameSpace
{
    public class Oracle : BaseEntity, IProposer
    {

        private List<int> _propositions;
        public Oracle() {}

        public void UpdateScore(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices){}

        public void MyTurn(){}

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
