using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using backend.Core.Domain.Games.Events;

namespace backend.Core.Domain.Games.Pipelines
{
    public class Guess
    {
        public record Request(Guid User, string Guess): IRequest<Unit> {}

        public class Handler: IRequestHandler<Request,Unit>
        {
            
            private readonly IGameService _service;
            private IMediator _mediator;
            public Handler(IGameService service, IMediator mediator)
            {
                _service = service ?? throw new ArgumentNullException(nameof(service));
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var game = _service.GetByUserId(request.User);

                if (game is null) return Unit.Value;
                
                var result = game.Guess(new GuessDto(){ User = request.User, Guess = request.Guess });


                if (!result) return Unit.Value;
                
                await _mediator.Publish(new CorrectGuessEvent(
                    game,
                    request.User,
                    request.Guess, 
                    game.Images.Count > 0,
                    game.VersusOracle
                ), cancellationToken);
                    


                return Unit.Value;
            }
        }
    }
}