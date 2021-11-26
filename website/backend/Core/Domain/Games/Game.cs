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
        public GameState State = GameState.Created;
        private DateTime _startTime = DateTime.Now;
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
        private Queue<Image> _images;
        private List<int> _proposals = new();

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
                    Guessers.ForEach(guesser => guesser.Guessed = false);
                    Events.Add(new GuessersTurnEvent() { PlayerIds = PlayerIds });
                }
                _proposersTurn = value;
            }
        }

        public Game(Guid id, List<Image> images, List<Guesser> guessers, IProposer proposer)
        {
            _proposersTurn = true;
            _images = new Queue<Image>(images);
            Id = id;
            Proposer = proposer;
            Guessers = guessers;
        }

        public void ConnectUser(Guid userId)
        {
            var guesser = Guessers.Find(g => g.Id == userId);

            if (guesser is not null)
            {
                guesser.Connected = true;
            }
        }

        public void DisconnectUser(Guid userId)
        {
            if (Proposer is Proposer && Proposer.GetId() == userId.ToString())
            {
                var oracle = new Oracle(Id);
                oracle.HandleNewImage(
                    CurrentImage.Slices
                        .Select(s => s.SequenceNumber)
                        .Except(_proposals)
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
            if (State == GameState.Created && (Guessers.All(g => g.Connected) || (_startTime + TimeSpan.FromSeconds(10)) <= DateTime.Now))
            {
                State = GameState.Active;
                NextImage();
                return;
            }

            if (State == GameState.Active && Guessers.All(g => !g.Connected))
            {
                GameOver();
                return;
            }

            if (!ProposersTurn && State == GameState.Active)
            {
                if ((_startTime + RoundTime) <= DateTime.Now)
                {
                    //Toggle role turn
                    ProposersTurn = true;
                }
            }
        }

        public void NextImage()
        {
            _images.TryDequeue(out CurrentImage);

            if (CurrentImage is null)
            {
                GameOver();
                return;
            }

            _proposals.Clear();

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


        public void Guess(GuessDto guess)
        {
            Guesser guesser = Guessers.Find(g => g.Id == guess.User && g.Connected);

            if (ProposersTurn || guesser.Guessed || CurrentImage is null)
            {
                return;
            }

            guesser.Guessed = true;

            Events.Add(new BroadcastGuessEvent()
            {
                PlayerIds = PlayerIds,
                Guess = guess.Guess,
                Username = guesser.Username,
            });


            // If guess was correct
            if (CurrentImage.Label.Label == guess.Guess)
            {
                var proposerScore = 0;

                if (Proposer is Proposer)
                {
                    proposerScore = Proposer.CalculateScore(RoundTime, DateTime.Now - _startTime, _proposals.Count, CurrentImage.Slices.Count);
                }

                var guesserScore = guesser.CalculateScore(RoundTime, DateTime.Now - _startTime, _proposals.Count, CurrentImage.Slices.Count);

                Events.Add(new CorrectGuessEvent()
                {
                    PlayerIds = PlayerIds.ToArray(),
                    Guesser = guesser,
                    GuesserScored = guesserScore,
                    Proposer = Proposer,
                    ProposerScored = proposerScore,
                    Guess = guess.Guess,
                    HasMoreRounds = _images.Count > 0,
                    WillAutoContinue = _images.Count > 0 && Proposer is Oracle,
                    Image = CurrentImage,
                });


                NextImage();
                return;
            }

            if (Guessers.Where(g => g.Connected).All(x => x.Guessed))
            {
                if (CurrentImage.Slices.Count == _proposals.Count)
                {
                    Events.Add(new FullyVisibleImageWithoutCorrectGuessesEvent()
                    {
                        PlayerIds = PlayerIds.ToArray(),
                        Guess = CurrentImage.Label.Label
                    });
                    NextImage();
                }
                else
                {
                    ProposersTurn = true;
                }
            }
        }


        public ImageSlice Propose(int proposal)
        {
            if (ProposersTurn && !_proposals.Contains(proposal) && (CurrentImage.Slices.Exists(x => x.SequenceNumber == proposal)))
            {
                _startTime = DateTime.Now;

                ProposersTurn = false;
                _proposals.Add(proposal);

                return CurrentImage.Slices.Find(i => i.SequenceNumber == proposal);
            }

            return null;
        }
    }
}