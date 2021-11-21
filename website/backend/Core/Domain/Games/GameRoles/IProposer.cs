

using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Core.Domain.Games
{
    public interface IProposer
    {
        int ScoreCalc(TimeSpan RoundTime,TimeSpan timeDelta,int slicesShown, int totalSlices, int nGuessers);
        void NotifyTurn();
        void HandleNewImage() {}
        string GetId();
        string GetUsername();

    }
}