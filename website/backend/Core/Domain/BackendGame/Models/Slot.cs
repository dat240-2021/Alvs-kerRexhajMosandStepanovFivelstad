using System;

namespace backend.Core.Domain.BackendGame.Models
{
    public record Slot
    {
        public Guid Id { get; set; }
        public SlotRole Role { get; set; }

        public Slot(Guid id, SlotRole role)
        {
            Id = id;
            Role = role;
        }
    }
}