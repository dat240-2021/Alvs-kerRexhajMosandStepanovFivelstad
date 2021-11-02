using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Authentication.Pipelines
{
        public class RegisterUser
        {
                public record Request(string Username, string Password) : IRequest<RegisterUser.Response>;

                public record Response(bool Success,string[] errors);

                public class Handler : IRequestHandler<Request,Response>
                {
                        private readonly IAuthenticationService _authServ;

                        public Handler(IAuthenticationService authServ) => _authServ = authServ ?? throw new ArgumentNullException(nameof(authServ));

                        public async Task<RegisterUser.Response> Handle(Request request, CancellationToken cancellationToken)
                        {
                                var result = await _authServ.RegisterUser(request.Username,request.Password);
                                return new Response(result.Success,result.errors);

                        }

                }
        }
}