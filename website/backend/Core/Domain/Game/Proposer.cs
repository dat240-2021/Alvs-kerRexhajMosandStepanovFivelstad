namespace backend.Core.Domain.Game
{
    public class Proposer
    {
        public Guid Id { get; set; }

        public Proposer(Guid id)
        {
            Id = id;
        }
    }
}