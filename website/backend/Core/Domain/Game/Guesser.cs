namespace backend.Core.Domain.Game
{
    public class Guesser
    {
        public Guid Id { get; set; }

        public Guesser(Guid id)
        {
            Id = id;
        }
    }
}