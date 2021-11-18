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

					string StripB64String(string fileString)
					{

						var ret = fileString;
						int b64Start = item.File.IndexOf(";base64,") + 8;
						if (b64Start > 8)
						{
							ret = fileString.Remove(0, fileString.IndexOf("base64,")+"base64,".Length );
						}
						//var ret = fileString.Remove(0, fileString.IndexOf("base64,")+"base64,".Length );
						return ret;
					}


					var cat = _db.ImageCategories.Where(x=> x.Id == item.Category).FirstOrDefault();

					List<byte[]> slicedList = null;
					if (item.SliceColors.Length>0 && item.SliceFile.Length>1){


						slicedList = new ManualSlicer().ManualSlice(
							Convert.FromBase64String(StripB64String(item.File)),
							Convert.FromBase64String(StripB64String(item.SliceFile)),
							item.SliceColors
							);

					} else{

						slicedList = new SliceImage().Slice(Convert.FromBase64String(StripB64String(item.File)));

					}

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

