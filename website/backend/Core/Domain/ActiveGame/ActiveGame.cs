using System;
using System.Linq;
using backend.Core.Domain.BackendGame.Models;
using Domain.Image;

namespace Core.Domain.ActiveGame{
    internal enum ActiveGameState
    {
        Propose,
        Guess,
    }
    public class ActiveGame {
        public Guid Id { get; protected set; }
        public GameSettings Settings { get; protected set; }
        private ActiveGameState State { get; set; }

        private int _currentImage { get; set; }
        public Image CurrentImage { get => Settings.Images.ElementAtOrDefault(_currentImage); }
        public DateTime StartTime;
        public TimeSpan RoundTime;

        public ActiveGame() {
            State = ActiveGameState.Propose;
        }

        private void ValidateRoundTime()
        {
            /// Allow propositions if round time has elapsed.
            if ((StartTime + RoundTime) >= DateTime.Now)
            {
                State = ActiveGameState.Propose;
            }
        }

        public string Guess(GuessDto guess)
        {
            ValidateRoundTime();

            Guesser guesser = Settings.Guessers.Find(g => g.Id == guess.User);
            guesser.Guessed = true;

            if (State == ActiveGameState.Guess)
            {
                /// Check valid guess
                /// If valid, add score to user and move to next image
                /// Else, register guess and check  
                if (CurrentImage.Label == guess.Guess) {
                    guesser.UpdateScore();
                    Settings.Proposer.UpdateScore();

                    AdvanceRound();
                }

                if (Settings.Guessers.All(x => x.Guessed))
                {
                    Advanceround();
                }
                return Guess.guess;
            }

            return null;
        }


        public void Propose(ProposeDto proposition)
        {
            ValidateRoundTime();

            if (State == ActiveGameState.Propose)
            {
                // Propose
                State = ActiveGameState.Guess;
            }
        }

    }


}