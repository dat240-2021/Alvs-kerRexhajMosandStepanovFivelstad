using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Core.Domain.BackendGame.Models
{
    public class GameSlotInfo
    {
        public List<Slot> PlayerSlots { get; set; } = new();
        public int MaxSlotsCount { get; set; }
        
        public ProposerType ProposerType { get; set; }
        
        public List<Guid> PlayerIds => PlayerSlots.Select(p => p.Id).ToList();
        
        public List<Guid> GuessersIds => PlayerSlots.Where(p => p.Role.Equals(SlotRole.Guesser)).Select(p => p.Id).ToList();
        public Guid? ProposerId => PlayerSlots.FirstOrDefault(p => p.Role.Equals(SlotRole.Proposer))?.Id;
    }
}