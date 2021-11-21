using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Authentication.Services;
using MediatR;

namespace Domain.Authentication.Pipelines
{
    public class GetCurrentUser
    {
        public record Request() : IRequest<User>;
        

        public class Handler : IRequestHandler<Request, User>
        {
            private readonly IAuthenticationService _authenticationService;

            public Handler(IAuthenticationService authServ) => _authenticationService = authServ ?? throw new ArgumentNullException(nameof(authServ));


            public async Task<User> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _authenticationService.GetCurrentUser();
            }
        }
    }
}