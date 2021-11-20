using System;
using System.Collections.Generic;
using System.Linq;
using backend.Core.Domain.Games.Events;
using backend.Core.Domain.Images;
using SharedKernel;

namespace backend.Core.Domain.Games
{
    public enum GameState
    {
        Created,
        Active,
        Ended,
    }
    public class Game : BaseEntity
    {
        public Guid Id { get; protected set; }
        private GameState State = GameState.Created;
        public DateTime StartTime = DateTime.Now;
        public TimeSpan RoundTime;

        public Image CurrentImage;


        public IProposer Proposer;
        public List<Guesser> Guessers;
        public List<String> GuesserIds
        {
            get
            {
                return Guessers.Where(g => g.Connected).Select(g => g.Id.ToString()).ToList();
            }
        }

        public List<string> PlayerIds
        {
            get
            {
                var list = GuesserIds;
                list.Add(Proposer.GetId());
                return list;
            }
        }
        public Queue<Image> Images;
        public List<int> SlicesShown = new();
        public int nProposes { get => SlicesShown.Count(); }

        private bool _proposersTurn;

        //When _proposersTurn is changed we have to send an event.
        private bool ProposersTurn
        {
            get => _proposersTurn; set
            {
                if (State != GameState.Active) return;

                if (value)
                {
                    Events.Add(new ProposersTurnEvent() { PlayerIds = PlayerIds });

                    if (Proposer is Oracle)
                    {
                        Events.Add(new OracleTurnEvent() { GameId = Id, Proposition = ((Oracle)Proposer).Proposal });
                    }
                }
                else
                {
                    Events.Add(new GuessersTurnEvent() { PlayerIds = PlayerIds });
                }
                _proposersTurn = value;
            }
        }

        public Game(Guid id, List<Image> images, List<Guesser> guessers, IProposer proposer)
        {
            _proposersTurn = true;
            Images = new Queue<Image>(images);
            Id = id;
            Proposer = proposer;
            Guessers = guessers;
        }

        public void ConnectUser(Guid userId)
        {
            Guessers.Find(g => g.Id == userId).Connected = true;
        }

        public void DisconnectUser(Guid userId)
        {
            if (Proposer is Proposer && Proposer.GetId() == userId.ToString())
            {
                var oracle = new Oracle(Id);
                oracle.HandleNewImage(
                    CurrentImage.Slices
                        .Select(s => s.SequenceNumber)
                        .Except(SlicesShown)
                        .ToList()
                );
                Proposer = oracle;

                if (ProposersTurn)
                {
                    Events.Add(new OracleTurnEvent() { GameId = Id, Proposition = ((Oracle)Proposer).Proposal });
                }
            }
            else
            {
                Guessers.Find(g => g.Id == userId).Connected = false;
            }
        }

        public void Update()
        {
            if (State == GameState.Created && (Guessers.All(g => g.Connected) || (StartTime + TimeSpan.FromSeconds(10)) <= DateTime.Now))
            {
                State = GameState.Active;
                NextImage();
                return;
            }

            if (State == GameState.Active && Guessers.All(g => !g.Connected))
            {
                GameOver();
            }

            if (!ProposersTurn && State == GameState.Active)
            {
                if ((StartTime + RoundTime) <= DateTime.Now)
                {
                    //Toggle role turn
                    ProposersTurn = true;
                }
            }
        }

        public void NextImage()
        {
            Images.TryDequeue(out CurrentImage);

            if (CurrentImage is null)
            {
                GameOver();
                return;
            }

            foreach (var g in Guessers)
            {
                g.Guessed = false;
            }

            SlicesShown.Clear();

            if (Proposer is Oracle)
            {
                ((Oracle)Proposer).HandleNewImage(CurrentImage.Slices.Select(slice => slice.SequenceNumber).ToList());
            }

            Events.Add(new NewImageEvent()
            {
                ImageId = CurrentImage.Id,
                GuesserIds = GuesserIds,
                ProposerId = Proposer.GetId()
            });

            ProposersTurn = true;
        }

        public void GameOver()
        {
            State = GameState.Ended;
            Events.Add(new GameOverEvent() { GameId = Id });
        }


        //returns bool, which implies this guess should be broadcast to all players
        public bool Guess(GuessDto guess)
        {
            Guesser guesser = Guessers.Find(g => g.Id == guess.User && g.Connected);

            if (!ProposersTurn && !guesser.Guessed && CurrentImage is not null)
            {
                guesser.Guessed = true;

                if (CurrentImage.Label.Label == guess.Guess)
                {

                    guesser.UpdateScore(RoundTime, DateTime.Now - StartTime, nProposes, CurrentImage.Slices.Count);
                    Proposer.UpdateScore(RoundTime, DateTime.Now - StartTime, nProposes, CurrentImage.Slices.Count, Guessers.Count);

                    NextImage();
                    return true;
                    //other guessers can keep guessing until time runs out.
                }

                if (Guessers.Where(g => g.Connected).All(x => x.Guessed))
                {
                    if (CurrentImage.Slices.Count == SlicesShown.Count)
                    {
                        NextImage();
                    }
                    else
                    {
                        ProposersTurn = true;
                        foreach (var g in Guessers)
                        {
                            g.Guessed = false;
                        }
                    }
                }

                // Implies valid guess -> broadcasted by hub
                return true;
            }
            return false;
        }


        public ImageSlice? Propose(int proposition)
        {
            if (ProposersTurn)
            {
                // Do proposer stuff
                if ((!SlicesShown.Contains(proposition)) && (CurrentImage.Slices.Exists(x => x.SequenceNumber == proposition)))
                {
                    StartTime = DateTime.Now;
                    foreach (var g in Guessers)
                    {
                        g.Guessed = false;
                    }
                    ProposersTurn = false;
                    SlicesShown.Add(proposition);
                    return CurrentImage.Slices.Find(i => i.SequenceNumber == proposition);
                }
            }
            return null;
        }
    }
}