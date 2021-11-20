using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Domain.Images.Pipelines
{
    public class GetImageIdsByCategory
    {
        public record Request(List<int> Categories,Guid UserId) : IRequest<List<int>>;

        public class Handler : IRequestHandler<Request, List<int>>
        {
            private readonly GameContext _db;

            public Handler(GameContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

            public async Task<List<int>> Handle(Request request, CancellationToken cancellationToken)
            {
                var categoryImageIdList = new List<int>();

                foreach (var category in request.Categories)
                {
                    var defaultList = await _db.Images
                        .Where(i => i.Label.Category.Id == category)
                        .Where(i => i.UserId == null || request.UserId == i.UserId)
                        .Select(i => i.Id).ToListAsync(cancellationToken);
                    categoryImageIdList.AddRange(defaultList);
                }
                return categoryImageIdList;
            }
        }
    }
}
