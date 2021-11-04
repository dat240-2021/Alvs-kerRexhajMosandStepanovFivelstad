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
	public class GetImagePieceById
	{
		public record Request(int Id, int SequenceNumber) : IRequest<byte[]?>;

		public class Handler : IRequestHandler<Request, byte[]?>
		{
			private readonly GameContext _db;

			public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

			public async Task<byte[]?> Handle(Request request, CancellationToken cancellationToken)
			{
				var image = await _db.Image.Include(i => i.ImageList).Where(i => i.Id == request.Id).SingleOrDefaultAsync();
				byte[]? imagePiece = null;
				foreach (var piece in image.ImageList)
				{
					if (piece.SequenceNumber == request.SequenceNumber)
					{
						imagePiece = piece.ImageData;
					}
				}

				//if (imagePiece is null) throw new EntityNotFoundException($"Order with Id {request.Category} was not found in the database");
				return imagePiece;
			}
		}
	}
}
