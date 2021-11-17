using System;

namespace backend.Core.Domain.Images.Utils
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private static int Next()
        {
            var random = new Random();
            return random.Next();
        }

        int IRandomNumberGenerator.Next()
        {
            return Next();
        }
    }

    public interface IRandomNumberGenerator
    {
        int Next();
    }
}