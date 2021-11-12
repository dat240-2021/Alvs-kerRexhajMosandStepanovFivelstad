using System;
using System.Collections.Generic;
using System.Linq;
using backend.Core.Domain.GameSpace.Events;
using Domain.Image;
using SharedKernel;

namespace backend.Core.Domain.GameSpace{
    // internal enum GameState
    // {
    //     Propose,
    //     Guess,
    // }
    public class Game<T> : BaseEntity where T : IProposer {
        public Guid Id { get; protected set; }
        // public GameSettings Settings { get; protected set; }
        public DateTime StartTime;
        public TimeSpan RoundTime;

        private int _currentImage { get; set; }
        public Image CurrentImage { get => Images.ElementAtOrDefault(_currentImage); }


        public T Proposer;
        public List<Guesser> Guessers;
        public List<Image> Images;
        public List<int> SlicesShown;
        public int nProposes {get => SlicesShown.Count(); }

        private bool _proposersTurn;

        //When _proposersTurn is changed we have to send an event.
        private bool ProposersTurn { get => _proposersTurn ; set {
                if (value)
                {
                    Proposer.MyTurn();
                } else
                {
                    Events.Add( new GuessersTurnEvent(){GuesserIds = Guessers.Select( g => g.Id.ToString()).ToList() });
                }
                _proposersTurn = value;
            }
        }

        public Game() {
            _currentImage = 0;
            Events.Add(new NewImageEvent()
                {
                    ImageId = CurrentImage.Id,
                    GuesserIds = Guessers.Select( g => g.Id.ToString()).ToList(),
                    ProposerId = Proposer.Id.ToString()
                }
            );
        }

        public void Update() {
            // what to do when we update.
            //checks if state has changed, and if so, sends an event.

            //If guesser time runs out of time.
            if (!ProposersTurn) {
                if (( StartTime + RoundTime) >= DateTime.Now) {
                    //Toggle role turn
                    ProposersTurn = true;
                }

                //If all the players have guessed
                if ( Guessers.All(x => x.Guessed) ) {
                    ProposersTurn = true;
                }
            }


            //if proposer times out
            // if () {
            //     Events.Add( new GuessersTurnEvent(){GuesserIds = Guessers.Select( g => g.Id.ToString()).ToList() });
            // }
        }

        //reset vars getting ready for next image.
        public void NextImage() {
            _currentImage++;
            if (_currentImage>=Images.Count)
            {
                GameOver();
                return;
            }
            foreach( var g in Guessers){
                g.Guessed = false;
            }
            SlicesShown.Clear();
        }

        public void GameOver(){
            Events.Add(new GameOverEvent(){GameId = Id});
        }


        //returns bool, which implies this guess should be broadcast to all players
        public bool Guess(GuessDto guess)
        {
            Guesser guesser = Guessers.Find(g => g.Id == guess.User);

            if (!ProposersTurn && !guesser.Guessed)
            {

                guesser.Guessed = true;

                if (CurrentImage.Label.Label == guess.Guess) {
                    // guesser.UpdateScore();
                    // Proposer.UpdateScore();

                    NextImage();

                    //other guessers can keep guessing until time runs out.
                    return true;
                }
                return true;
            }
            return false;
        }


        public ImageSlice? Propose(ProposeDto proposition)
        {
            if (ProposersTurn)
            {
                // Do proposer stuff
                if ( ( ! SlicesShown.Contains(proposition.SliceNumber) )  && (CurrentImage.Slices.Exists(x => x.SequenceNumber == proposition.SliceNumber) ))
                {
                    StartTime = DateTime.Now;
                    ProposersTurn = false;
                    return CurrentImage.Slices.Find(i => i.SequenceNumber == proposition.SliceNumber);
                }
            }
            return null;
        }
    }
}