using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Image.Pipelines
{
	public class GetImageById
	{
		public record Request(int Id) : IRequest<Image?>;

		public class Handler : IRequestHandler<Request, Image?>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<Image?> Handle(Request request, CancellationToken cancellationToken)
			{
				var image = await _db.Image.Include(i => i.ImageList).Where(i => i.Id == request.Id).SingleOrDefaultAsync();
				//if (imagePiece is null) throw new EntityNotFoundException($"Order with Id {request.Category} was not found in the database");
				return image;
			}
		}
	}
}
