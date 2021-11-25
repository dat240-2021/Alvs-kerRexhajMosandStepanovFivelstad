using System;
using System.Collections.Generic;
using backend.Core.Domain.Games;
using backend.Core.Domain.Games.Events;
using backend.Core.Domain.Images;
using backend.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace backend.Tests.Core.Domain.Games
{
    public class GameContextTests : DbTest
    {
        public GameContextTests(ITestOutputHelper output) : base(output)
        {
        }

        //Test AddUserImage pipeline by adding one image. 
        [Fact]
        public void Guessing_Fails_When_Proposers_Turn()
        {
            var user = Guid.NewGuid();
            var game = new Game(
                Guid.NewGuid(),
                new List<backend.Core.Domain.Images.Image>() { new (Guid.NewGuid(), new ImageLabel("test", new ImageCategory(2, "test"))) },
                new List<Guesser>() { new Guesser(user, "") { Connected = true } },
                new Proposer(Guid.NewGuid())
            );
            game.Update();

            game.Guess(new GuessDto() { User = user, Guess = "guess" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 0);

        }

        [Fact]
        public void Guessing_Succeeds_When_Guessers_Turn()
        {
            var user = Guid.NewGuid();
            var image = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test", new ImageCategory(2, "test")));
            image.AddImageSlice(new byte[2], 3);

            var game = new Game(
                Guid.NewGuid(),
                new() { image },
                new List<Guesser>() { new Guesser(user,"") { Connected = true } },
                new Proposer(Guid.NewGuid())
            );
            game.Update();

            game.Propose(3);
            game.Guess(new GuessDto() { User = user, Guess = "guess" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 1);
        }

        [Fact]
        public void SingleGuesser_SingleImage()
        {
            var user = Guid.NewGuid();
            var image = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test", new ImageCategory(2, "test")));
            image.AddImageSlice(new byte[2], 3);

            var game = new Game(
                Guid.NewGuid(),
                new() { image },
                new List<Guesser>() { new Guesser(user,"") { Connected = true } },
                new Proposer(Guid.NewGuid())
            );
            game.Update();

            game.Propose(3);
            game.Guess(new GuessDto() { User = user, Guess = "test" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 1);

            Assert.True(game.State == GameState.Ended);
        }

        [Fact]
        public void SingleGuesser_MultipleImages()
        {
            var user = Guid.NewGuid();
            var image1 = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test", new ImageCategory(2, "test")));
            var image2 = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test2", new ImageCategory(2, "test")));
            var image3 = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test3", new ImageCategory(2, "test")));
            image1.AddImageSlice(new byte[2], 1);
            image2.AddImageSlice(new byte[2], 1);
            image3.AddImageSlice(new byte[2], 1);

            var game = new Game(
                Guid.NewGuid(),
                new() { image1, image2, image3 },
                new List<Guesser>() { new Guesser(user,"") { Connected = true } },
                new Proposer(Guid.NewGuid())
            );
            game.Update();

            // Propose tile 1 of image1
            game.Propose(1);

            // Valid guess. Guess is also correct.
            game.Guess(new GuessDto() { User = user, Guess = "test" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 1);

            Assert.DoesNotContain(game.Events, x => x is GameOverEvent);

            // Propose tile 1 of image2
            game.Propose(1);

            // Valid guess, even though guess is incorrect.
            game.Guess(new GuessDto() { User = user, Guess = "test2" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 2);
            Assert.DoesNotContain(game.Events, x => x is GameOverEvent);

            // Propose tile 1 of image3
            game.Propose(1);

            // Valid guess, even though guess is incorrect.
            game.Guess(new GuessDto() { User = user, Guess = "test" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 3);

            // No more tiles to propose. Game ends.
            Assert.True(game.State == GameState.Ended);
        }

        [Fact]
        public void MultipleGuessers_SingleImage()
        {
            var user1 = Guid.NewGuid();
            var user2 = Guid.NewGuid();
            var image = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test", new ImageCategory(2, "test")));
            image.AddImageSlice(new byte[2], 1);

            var game = new Game(
                Guid.NewGuid(),
                new() { image },
                new List<Guesser>() { new Guesser(user1,"") { Connected = true }, new Guesser(user2,"") { Connected = true } },
                new Proposer(Guid.NewGuid())
            );
            game.Update();

            // Propose tile 1 of image1
            game.Propose(1);

            // Valid and correct guess.
            game.Guess(new GuessDto() { User = user2, Guess = "test" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 1);


            // Game has now ended, so further guesses are denied.
            game.Guess(new GuessDto() { User = user1, Guess = "test1" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 1);

            Assert.True(game.State == GameState.Ended);
        }

        [Fact]
        public void MultipleGuessers_MultipleImages()
        {
            var user1 = Guid.NewGuid();
            var user2 = Guid.NewGuid();
            var user3 = Guid.NewGuid();
            var image1 = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test", new ImageCategory(2, "test")));
            var image2 = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test2", new ImageCategory(2, "test")));
            var image3 = new backend.Core.Domain.Images.Image(Guid.NewGuid(), new ImageLabel("test3", new ImageCategory(2, "test")));
            image1.AddImageSlice(new byte[2], 1);
            image2.AddImageSlice(new byte[2], 1);
            image3.AddImageSlice(new byte[2], 1);

            var game = new Game(
                Guid.NewGuid(),
                new() { image1, image2, image3 },
                new List<Guesser>() { new Guesser(user1,"") { Connected = true }, new (user2,"") { Connected = true }, new (user3,"") { Connected = true } },
                new Proposer(Guid.NewGuid())
            );
            game.Update();

            // Propose tile 1 of image1
            game.Propose(1);

            // Valid guess. Guess is also correct.
            game.Guess(new GuessDto() { User = user1, Guess = "test" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 1);


            // Previous guess was correct. This guess fails because it's the proposers turn now.
            game.Guess(new GuessDto() { User = user2, Guess = "test" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 1);
            Assert.DoesNotContain(game.Events, x => x is GameOverEvent);

            // Propose tile 1 of image2
            game.Propose(1);

            // Valid guess, even though guess is incorrect.
            game.Guess(new GuessDto() { User = user1, Guess = "test2" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 2);

            Assert.False(game.State == GameState.Ended);

            // Propose tile 1 of image3
            game.Propose(1);
            game.Guess(new GuessDto() { User = user2, Guess = "test2" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 3);

            game.Guess(new GuessDto() { User = user3, Guess = "test2" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 4);


            // Valid guess, even though guess is incorrect.
            game.Guess(new GuessDto() { User = user1, Guess = "test" });
            Assert.True(game.Events.FindAll(x => x is BroadcastGuessEvent).ToArray().Length == 5);


            // No more tiles to propose. Game ends.
            Assert.True(game.Propose(1) is null);
            Assert.True(game.State == GameState.Ended);
        }
    }
}
