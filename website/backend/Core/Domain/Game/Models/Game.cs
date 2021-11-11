//This is placed here temporarily, will be moved if needed. 


namespace backend.Core.Domain.Game {
    
    public record Game {
        public IProposer Proposer {get; set;}

        public List<Guesser> Guessers {get; set;}


    }






}