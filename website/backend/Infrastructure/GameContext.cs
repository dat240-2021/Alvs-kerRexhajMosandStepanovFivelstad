using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Pipelines;
using Domain.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

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
		public DbSet<Score> Scores { get; set; } = null!;
		public DbSet<Game> Games { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);
        }
	}

}
