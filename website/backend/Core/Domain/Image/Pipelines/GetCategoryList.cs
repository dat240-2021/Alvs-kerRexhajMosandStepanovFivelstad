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
	public class GetCategoryList
	{
		public record Request() : IRequest<List<string>>;

		public class Handler : IRequestHandler<Request, List<string>>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<List<string>> Handle(Request request, CancellationToken cancellationToken)
			{
				var categoryList = await _db.ImageCategories.Select(i => i.Name).ToListAsync(cancellationToken);
				return categoryList;
			}
		}
	}
}
