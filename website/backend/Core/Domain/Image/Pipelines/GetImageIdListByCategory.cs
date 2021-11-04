using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Image.Pipelines
{
	public class GetImageIdListByCategory
	{
		public record Request(string Category) : IRequest<List<int>?>;

		public class Handler : IRequestHandler<Request, List<int>?>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<List<int>?> Handle(Request request, CancellationToken cancellationToken)
			{
				var categoryImageIdList = await _db.Image.Include(ic => ic.Categories.Category).Where(i => i.Categories.Category == request.Category).Select(i => i.Id).ToListAsync(cancellationToken);
				//if (categoryImageIdList is null) throw new EntityNotFoundException($"Order with Id {request.Category} was not found in the database");
				return categoryImageIdList;
			}
		}
	}
}
