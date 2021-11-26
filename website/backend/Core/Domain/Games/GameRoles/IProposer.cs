

using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Core.Domain.Games
{
    public interface IProposer
    {
        int Score { get; }
        string Username { get; }
        int CalculateScore(TimeSpan RoundTime, TimeSpan timeDelta, int slicesShown, int totalSlices);
        void NotifyTurn();
        void HandleNewImage() { }
        string GetId();

    }
}