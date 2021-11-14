//This is placed here temporarily, will be moved if needed. 
using System;
using System.Collections.Generic;
using Domain.Image;

namespace backend.Core.Domain.Games {
    public record ProposeDto {

        public Guid ProposerId {get; set;}
        public int SliceNumber {get; set;}

    }
}