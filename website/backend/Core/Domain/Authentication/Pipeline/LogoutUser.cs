using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Authentication.Pipelines
{
    public class LogoutUser
    {
        public record Request() : IRequest<Unit>;

        public class Handler : IRequestHandler<Request>
        {
            private readonly IAuthenticationService _authServ;

            public Handler(IAuthenticationService authServ) => _authServ = authServ ?? throw new ArgumentNullException(nameof(authServ));


            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                await _authServ.LogoutUser();
                return Unit.Value;
            }
        }
    }
}