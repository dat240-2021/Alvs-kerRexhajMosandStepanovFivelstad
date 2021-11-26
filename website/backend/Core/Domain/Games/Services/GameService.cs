using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MediatR;

namespace backend.Core.Domain.Games
{
    public interface IGameService
    {
        Game Get(Guid gameId);
        Game GetByUserId(Guid userId);
        void RemoveUser(Guid userId);
        bool Add(Game game);
        Game Remove(Guid gameId);
    }

    public class GameService : IGameService
    {
        private ConcurrentDictionary<Guid, Game> Games = new();
        private ConcurrentDictionary<Guid, Guid> GameIdsByUsers = new();
        private readonly IMediator _mediator;

        private System.Timers.Timer Timer;

        public GameService(IMediator mediator)
        {
            _mediator = mediator;
            StartTicker();
        }

        //This is run when an event for a new game is handled...
        public Game Get(Guid gameId)
        {
            Game active_game = null;

            Games.TryGetValue(gameId, out active_game);
            return active_game;
        }
        private async void UpdateGames(Object _, ElapsedEventArgs e)
        {
            foreach (var game in Games.Values)
            {
                game.Update();

                var events = game.Events.ToArray();
                game.Events.Clear();

                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent);
                }

            }
        }

        private void StartTicker()
        {
            Timer = new System.Timers.Timer(500);
            Timer.Elapsed += UpdateGames;
            Timer.Enabled = true;
        }

        public Game GetByUserId(Guid userId)
        {
            GameIdsByUsers.TryGetValue(userId, out var gameId);
            Games.TryGetValue(gameId, out var game);
            return game;
        }

        public void RemoveUser(Guid userId)
        {
            GameIdsByUsers.TryRemove(userId, out var gameId);
            Game game = Get(gameId);
            if (game is not null)
            {
                game.DisconnectUser(userId);
            }
        }

        public bool Add(Game game)
        {
            foreach (Guesser guesser in game.Guessers)
            {
                /// Test that a user cannot already exist here.
                GameIdsByUsers.TryAdd(guesser.Id, game.Id);
            }

            if (game.Proposer is Proposer)
            {
                GameIdsByUsers.TryAdd(((Proposer)game.Proposer).Id, game.Id);
            }
            return Games.TryAdd(game.Id, game);
        }


        public Game Remove(Guid gameId)
        {
            foreach (Guid guesser in GameIdsByUsers.Where(g => g.Value == gameId).Select(g => g.Key))
            {
                GameIdsByUsers.TryRemove(guesser, out _);
            }

            Games.TryRemove(gameId, out var game);

            return game;
        }
    }
}