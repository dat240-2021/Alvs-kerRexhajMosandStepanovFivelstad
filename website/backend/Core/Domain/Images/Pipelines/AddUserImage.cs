using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Images.ImageSliceHelpers;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.Images.Pipelines
{
	public class AddUserImage
	{
		public record Request(List<(byte[],string, string)> ImageList, Guid UserId) : IRequest<Response>;

		public record Response(bool Success);

		public class Handler : IRequestHandler<Request,Response>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				foreach (var item in request.ImageList)
				{
					var tempCategory = _db.ImageCategories.SingleOrDefault(i => i.Name == item.Item3);
					if (tempCategory is null)
					{
						tempCategory = new ImageCategory(item.Item3);
						_db.ImageCategories.Add(tempCategory);
					}
					
					var slicedList = new SliceImage().Slice(item.Item1);
					var image = new global::backend.Core.Domain.Images.Image(request.UserId,new ImageLabel(item.Item2,tempCategory));
					var sequenceNumber = 0;
					foreach (var slice in slicedList)
					{
						image.AddImageSlice(slice,sequenceNumber);
						sequenceNumber++;
					}
					_db.Add(image);
					
				}
				
				await _db.SaveChangesAsync();

				return new Response(Success: true);

			}
		}
	}
}

