using System.Linq;
using System.Reflection;
using System.Threading;
using Domain.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class GameContext : DbContext
	{
		private readonly IMediator _mediator;

		public GameContext(DbContextOptions configuration, IMediator mediator) : base(configuration)
		{
			_mediator = mediator;
		}

		public DbSet<User> Users {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);
        }
    }

}
