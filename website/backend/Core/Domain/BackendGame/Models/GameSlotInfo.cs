using System;
using System.Collections.Generic;

namespace backend.Core.Domain.BackendGame.Models
{
    public class GameSlotInfo
    {
        public List<Guid> Players { get; set; } = new();
        public int MaxSlotsCount { get; set; }
    }
}