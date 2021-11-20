using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Core.Domain.Images.Utils;
using Infrastructure.Data;
using MediatR;

// NEED TO REWRITE THIS. ITS A TEMP SOLUTION FOR THIS PR
namespace backend.Core.Domain.Images.Pipelines
{
    public class GetImageIdsListByCategoriesIds
    {
        public record Request(List<int> CategoryIds, int? ImagesCount, Guid UserId) : IRequest<List<int>>;

        public class Handler : IRequestHandler<Request, List<int>>
        {
            private readonly GameContext _db;
            private readonly IMediator _mediator;
            private readonly IRandomNumberGenerator _rnd;

            public Handler(GameContext db, IMediator mediator, IRandomNumberGenerator rnd)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _rnd = rnd;
            }

            public async Task<List<int>> Handle(Request request, CancellationToken cancellationToken)
            {
                var imageCategories = await _mediator.Send(new GetCategoryList.Request());
                var categories = imageCategories
                    .Where(ic => request.CategoryIds.Contains(ic.Id))
                    .Select(c => c.Id)
                    .ToList();
                var ids = await _mediator.Send(new GetImageIdsByCategory.Request(categories, request.UserId), cancellationToken);
                var randomizedIds = ids.OrderBy(_ => _rnd.Next()).ToList();

                return request.ImagesCount is null ?
                    randomizedIds :
                    randomizedIds.Take((int)request.ImagesCount).ToList();
            }
        }
    }
}