using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using backend.Hubs;

namespace backend.Core.Domain.Games.Pipelines
{
    public class Disconnect
    {
        public record Request(Guid User): IRequest<Unit> {}

        public class Handler: IRequestHandler<Request, Unit>
        {
            private IGameService _service;
            public Handler(IGameService service)
            {
                _service = service ?? throw new System.ArgumentNullException(nameof(service));
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _service.RemoveUser(request.User);
  
                return Task.FromResult(Unit.Value);
            }
        }
    }
}