using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.ActiveGame{
    public interface IActiveGameService {

    }

    public class ActiveGameService : IActiveGameService{
        private ConcurrentDictionary<Guid,ActiveGame> Games;




        //This is run when an event for a new game is handled...
        public ActiveGame Get(Guid gameId){
            ActiveGame active_game = null;

            Games.TryGetValue(gameId, out active_game);
            return active_game;
        }


        public bool Add(ActiveGame game){
            return Games.TryAdd(game.Id,game);
        }


        public bool Remove(Guid gameId){
            return Games.TryRemove(gameId,out _);
        }
    }
}