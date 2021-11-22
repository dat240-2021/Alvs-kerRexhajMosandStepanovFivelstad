//This is placed here temporarily, will be moved if needed. 
using System;
using System.Collections.Generic;

namespace backend.Core.Domain.Games {
    public record GuessDto {

        public Guid User {get; set;}
        public string Guess {get; set;}
        public string Username {get; set;}


    }
}