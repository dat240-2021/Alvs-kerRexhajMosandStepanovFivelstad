using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Controllers.NewGame.Dto;
using backend.Core.Domain.Images.ImageSliceHelpers;
using Infrastructure.Data;
using MediatR;

namespace backend.Core.Domain.Images.Pipelines
{
	public class AddUserImage
	{
		public record Request(ImageFile[] ImageList, Guid UserId) : IRequest<Response>;

		public record Response(bool Success);

		public class Handler : IRequestHandler<Request,Response>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				foreach (var item in request.ImageList)
				{

					var cat = _db.ImageCategories.Where(x=> x.Id == item.Category).FirstOrDefault();

					var b64 = item.File;
					int b64Start = item.File.IndexOf(";base64,") + 8;
					if (b64Start > 8)
					{
						b64 = item.File.Substring(b64Start);
					}

					//var b64 = item.File.Remove(0,"data:image/jpeg;base64,".Length);

					var slicedList = new SliceImage().Slice(Convert.FromBase64String(b64));
					var image = new Image(request.UserId,new ImageLabel(item.Label,cat));
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

