

using System;
using System.Collections.Generic;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Services;
using backend.Tests.Helpers;
using Domain.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace backend.Tests.Core.Domain.BackendGame
{
    public class BackendGameServiceTest: DbTest
    {
        public BackendGameServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void UserJoinsNotStoredGameWillFailTest()
        {
            var settings = GetGameSettings(new List<int>{1, 2, 3}, 10, 1, 1, "AI");
            var user = new User();

            var game = GetGame(settings, user);
            
            var service = new BackendGameService();

            void Act() => service.JoinGame(game.Id, user.Id, SlotRole.Guesser);
            Assert.Throws<Exception>(Act);
        }
        
        [Fact]
        public void UserJoinsGameTest()
        {
            var settings = GetGameSettings(new List<int>{1, 2, 3}, 10, 1, 1, "AI");
            var userId = Guid.NewGuid();
            var user = new User()
            {
                Id = userId
            };
            
            var game = GetGame(settings, user);

            var service = new BackendGameService();
            service.StoreGame(game);
            
            Assert.True(service.HasAvailableSlots(game.Id));

            service.JoinGame(game.Id, user.Id, SlotRole.Guesser);
            Assert.False(service.HasAvailableSlots(game.Id));

            var slotInfo = service.GetSlotInfo(game.Id);
            Assert.Contains(userId, slotInfo.GuessersIds);
        }
        
        
        [Fact]
        public void UserLeavesNotJoinedGameWillFailTest()
        {
            var settings = GetGameSettings(new List<int>{1, 2, 3}, 10, 1, 1, "AI");
            var user = new User();
            var game = GetGame(settings, user);
            
            var service = new BackendGameService();
            service.StoreGame(game);

            void Act() => service.LeaveGame(game.Id, user.Id);
            Assert.Throws<Exception>(Act);
        }
        
        [Fact]
        public void UserLeavesGameTest()
        {
            var settings = GetGameSettings(new List<int>{1, 2, 3}, 10, 1, 1, "AI");
            var user = new User();
            var game = GetGame(settings, user);
            
            var service = new BackendGameService();
            service.StoreGame(game);
            
            service.JoinGame(game.Id, user.Id, SlotRole.Guesser);
            Assert.False(service.HasAvailableSlots(game.Id));

            service.LeaveGame(game.Id, user.Id);
            Assert.True(service.HasAvailableSlots(game.Id));
        }
        
        
        private static GameSettings GetGameSettings(List<int> categoryIds, int duration, int guessers, int images, string proposer)
        {
            return new()
            {
                CategoryIds = categoryIds,
                Duration = duration,
                GuessersCount = guessers,
                ImagesCount = images,
                ProposerType = proposer
            };
        }

        private static Game GetGame(GameSettings settings, User user)
        {
            return new(
                Guid.NewGuid(),
                settings,
                user
            );
        }
    }
}