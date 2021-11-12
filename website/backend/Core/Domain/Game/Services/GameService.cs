using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace backend.Core.Domain.GameSpace {
    public interface IGameService {
        Game Get(Guid gameId);
        Game GetByUserId(Guid userId);
        bool Add(Game game);
        bool Remove(Guid gameId);
    }

    public class GameService : IGameService{
        private ConcurrentDictionary<Guid, Game> Games;
        private ConcurrentDictionary<Guid, Guid> GameIdsByUsers;

        private Thread Ticker ;

        // Cleanup statements, preventing memory leaks etc.
        ~GameService()  // finalizer
        {
            Ticker.Abort();
        }

        //This is run when an event for a new game is handled...
        public Game Get(Guid gameId){
            Game active_game = null;

            Games.TryGetValue(gameId, out active_game);
            return active_game;
        }
        private void UpdateGames() {
            foreach (var game in Games.Values){
                game.Update();
            }
        }
        private void StartTicker(string[] args) {
                Ticker = new Thread(() => {
                    while (true) {
                        UpdateGames();
                        Thread.Sleep(1000);
                    }
                } )
                {
                    IsBackground = true
                };
                Ticker.Start();
            }

        public Game GetByUserId(Guid userId)
        {
            Games.TryGetValue(userId, out var game);
            return game;
        }

        public bool Add(Game game){
            foreach (Guesser guesser in game.Guessers)
            {
                /// Test that a user cannot already exist here.
                GameIdsByUsers.TryAdd(guesser.Id, game.Id);
            }
            return Games.TryAdd(game.Id,game);
        }


        public bool Remove(Guid gameId){
            foreach (Guid guesser in GameIdsByUsers.Where(g => g.Value == gameId).Select(g => g.Key))
            {
                GameIdsByUsers.TryRemove(guesser, out _);
            }

            return Games.TryRemove(gameId, out _);
        }
    }
}