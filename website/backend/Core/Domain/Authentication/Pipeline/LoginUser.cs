using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Authentication.Pipelines
{
        public class LoginUser
        {
                public record Request(string Username, string Password) : IRequest<LoginUser.Response>;

                public record Response(bool Success);

                public class Handler : IRequestHandler<Request,Response>
                {
                        private readonly IAuthenticationService _authServ;

                        public Handler(IAuthenticationService authServ) => _authServ = authServ ?? throw new ArgumentNullException(nameof(authServ));


                        public async Task<LoginUser.Response> Handle(Request request, CancellationToken cancellationToken)
                        {
                                return new Response( Success : await _authServ.LoginUser(request.Username,request.Password) );
                        }
                }
        }
}