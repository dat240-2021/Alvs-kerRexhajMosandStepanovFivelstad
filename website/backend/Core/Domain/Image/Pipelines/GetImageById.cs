using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Image.Pipelines
{
	public class GetImageById
	{
		public record Request(int Id) : IRequest<backend.Core.Domain.Image.Image>;

		public class Handler : IRequestHandler<Request, backend.Core.Domain.Image.Image>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<backend.Core.Domain.Image.Image> Handle(Request request, CancellationToken cancellationToken)
			{
				var image = await _db.Images.Include(i => i.Slices).Where(i => i.Id == request.Id).SingleOrDefaultAsync();
				return image;
			}
		}
	}
}
