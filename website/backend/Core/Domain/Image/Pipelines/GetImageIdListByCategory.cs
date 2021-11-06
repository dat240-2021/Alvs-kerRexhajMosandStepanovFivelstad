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
		//finish .....
		public record Request(List<string> Categories, Guid? UserId) : IRequest<List<int>>;

		public class Handler : IRequestHandler<Request, List<int>>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<List<int>> Handle(Request request, CancellationToken cancellationToken)
			{
				var categoryImageIdList = new List<int>();
				if (request.UserId != null)
				{
					var userList = await _db.Image.Include(ic => ic.Category.Category).Where(i => i.Category.Category == "My Images").Where(i => i.UserId == request.UserId).Select(i => i.Id).ToListAsync(cancellationToken);
					categoryImageIdList.AddRange(userList);
				}

				foreach (var category in request.Categories)
				{
					var tempList = await _db.Image.Include(ic => ic.Category.Category).Where(i => i.Category.Category == category).Select(i => i.Id).ToListAsync(cancellationToken);
					categoryImageIdList.AddRange(tempList);
				}
				return categoryImageIdList;
			}
		}
	}
}
