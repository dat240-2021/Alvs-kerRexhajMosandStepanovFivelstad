using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Domain.Authentication.Pipelines;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Image.Pipelines
{
	public class AddUserImage
	{
		public record Request(List<(byte[],int)> ImageList, Guid UserId, string ImageName) : IRequest<Response>;

		public record Response(bool Success);

		public class Handler : IRequestHandler<Request,Response>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				var tempCategory = _db.ImageCategories.SingleOrDefault(i => i.Category == "My Images");
				if (tempCategory is null)
				{
					tempCategory = new ImageCategory("My Images");
					_db.ImageCategories.Add(tempCategory);
				}
				var image = new Image(request.ImageName,tempCategory,request.UserId);
				foreach (var item in request.ImageList)
				{
					var tempPiece = new ImageSlice(item.Item1, item.Item2);
					image.Slices.Add(tempPiece);
				}

				_db.Add(image);
				await _db.SaveChangesAsync();

				return new Response(Success: true);

			}
		}
	}
}
