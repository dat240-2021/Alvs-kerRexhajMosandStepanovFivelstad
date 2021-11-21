using System;
using Domain.Authentication;

namespace backend.Core.Domain.Lobby.Models
{
    public class Score
    {
        public int Id { get; set; }
        public Guid User { get; set; }
        public int UserScore { get; set; }
        public DateTime Date { get; set; }

        public  Score(){}


        public Score(Guid usr,int score){
            User = usr;
            UserScore = score;
            Date = DateTime.Now;
        }
    }

}