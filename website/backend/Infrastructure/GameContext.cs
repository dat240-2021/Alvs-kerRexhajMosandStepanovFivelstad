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
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
	        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

	        // ignore events if no dispatcher provided
	        if (_mediator == null) return result;

	        // dispatch events only if save was successful
	        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
		        .Select(e => e.Entity)
		        .Where(e => e.Events.Any())
		        .ToArray();

	        foreach (var entity in entitiesWithEvents)
	        {
		        var events = entity.Events.ToArray();
		        entity.Events.Clear();
		        foreach (var domainEvent in events)
		        {
			        await _mediator.Publish(domainEvent, cancellationToken);
		        }
	        }
	        return result;
        }
    }

}
