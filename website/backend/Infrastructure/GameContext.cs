using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Domain.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class GameContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		private readonly IMediator _mediator;

		public GameContext(DbContextOptions configuration, IMediator mediator) : base(configuration)
		{
			_mediator = mediator;
		}

		// public DbSet<User> Users {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);
        }
    }

}
