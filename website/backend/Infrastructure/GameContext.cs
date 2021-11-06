using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using backend.Core.Domain.Image;
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
		public DbSet<ImagePiece> ImagePiece { get; set; } = null!;
		public DbSet<Image> Image { get; set; } = null!;
		public DbSet<ImageCategory> ImageCategory { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);
        }
    }

}
