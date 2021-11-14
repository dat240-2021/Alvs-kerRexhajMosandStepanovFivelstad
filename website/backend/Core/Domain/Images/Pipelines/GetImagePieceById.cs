using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Images.Pipelines
{
    public class GetImagePieceById
    {
        public record Request(int Id, int SequenceNumber) : IRequest<byte[]>;

        public class Handler : IRequestHandler<Request, byte[]>
        {
            private readonly GameContext _db;

            public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

            public async Task<byte[]> Handle(Request request, CancellationToken cancellationToken)
            {
                var image = await _db.Images.Include(i => i.Slices).Where(i => i.Id == request.Id).SingleOrDefaultAsync();
                if (image!=null){

                return image.Slices.Find(i => i.SequenceNumber == request.SequenceNumber).ImageData;

                }

                return null;
            }
        }
    }
}
