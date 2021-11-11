using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Game{
    public interface IActiveGameService {

    }

    public class ActiveGameService : IActiveGameService{
        private ConcurrentDictionary<Guid,Game> Games;




        //This is run when an event for a new game is handled...
        public Game Get(Guid gameId){
            Game active_game = null;

            Games.TryGetValue(gameId, out active_game);
            return active_game;
        }


        public bool Add(Game game){
            return Games.TryAdd(game.Id,game);
        }


        public bool Remove(Guid gameId){
            return Games.TryRemove(gameId,out _);
        }
    }
}