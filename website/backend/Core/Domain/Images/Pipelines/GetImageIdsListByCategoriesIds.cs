using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;

// NEED TO REWRITE THIS. ITS A TEMP SOLUTION FOR THIS PR
namespace backend.Core.Domain.Images.Pipelines
{
    public class GetImageIdsListByCategoriesIds
    {
        public record Request(List<int> CategoryIds, int? ImagesCount) : IRequest<List<int>>;

        public class Handler : IRequestHandler<Request, List<int>>
        {
            private readonly GameContext _db;
            private readonly IMediator _mediator;

            public Handler(GameContext db, IMediator mediator, Random rnd)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<List<int>> Handle(Request request, CancellationToken cancellationToken)
            {
                var imageCategories = await _mediator.Send(new GetCategoryList.Request());
                var categories = imageCategories
                    .Where(ic => request.CategoryIds.Contains(ic.Id))
                    .Select(c => c.Name)
                    .ToList();
                var ids = await _mediator.Send(new GetImageIdListByCategory.Request(categories, null), cancellationToken);
                
                var rnd = new Random();
                var randomizedIds = ids.OrderBy(_ => rnd.Next()).ToList();

                return request.ImagesCount is null ? 
                    randomizedIds : 
                    randomizedIds.Take((int)request.ImagesCount).ToList();
            }
        }
    }
}