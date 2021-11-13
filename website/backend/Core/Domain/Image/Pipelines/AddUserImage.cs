using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Image;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.Image.Pipelines
{
	public class AddUserImage
	{
		public record Request(List<(byte[],int)> ImageList, Guid UserId, string ImageLabel, string Category) : IRequest<Response>;

		public record Response(bool Success);

		public class Handler : IRequestHandler<Request,Response>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				var tempCategory = _db.ImageCategories.SingleOrDefault(i => i.Name == request.Category);
				if (tempCategory is null)
				{
					tempCategory = new ImageCategory(request.Category);
					_db.ImageCategories.Add(tempCategory);
				}

				var image = new global::backend.Core.Domain.Image.Image(request.UserId,new ImageLabel(request.ImageLabel,tempCategory));
				foreach (var item in request.ImageList)
				{
					image.AddImageSlice(item.Item1,item.Item2);
				}

				_db.Add(image);
				await _db.SaveChangesAsync();

				return new Response(Success: true);

			}
		}
	}
}
