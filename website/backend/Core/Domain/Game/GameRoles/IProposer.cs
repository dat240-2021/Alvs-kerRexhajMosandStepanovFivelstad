

using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Core.Domain.GameSpace
{
    public interface IProposer
    {
        void UpdateScore(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices);
        void MyTurn();
        void HandleNewImage() {}
        string? GetId();

        
    }
}