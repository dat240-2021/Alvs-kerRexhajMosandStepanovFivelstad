using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Game{
    public interface IGameService {
        Game Get(Guid gameId);
        Game GetByUserId(Guid userId);
        bool Add(Game game);
        bool Remove(Guid gameId);
    }

    public class GameService : IGameService{
        private ConcurrentDictionary<Guid, Game> Games;
        private ConcurrentDictionary<Guid, Guid> GameIdsByUsers;

        //This is run when an event for a new game is handled...
        public Game Get(Guid gameId){
            Game active_game = null;

            Games.TryGetValue(gameId, out active_game);
            return active_game;
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
                GamesByUsers.TryAdd(guesser.Id, game.Id);
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