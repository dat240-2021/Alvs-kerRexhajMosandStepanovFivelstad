using System.Threading.Tasks;
using backend.Controllers.NewGame.Dto;
using Controllers.Authentication;
using Controllers.Generics;
using Domain.Image.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // using Microsoft.AspNetCore.Http.StatusCode;

namespace backend.Controllers.NewGame
{

    [ApiController]
    // [Route("[controller]")]
    [Route("/api/category")]
    public class CategoryController : ApiBaseController
    {
        private readonly ILogger<CategoryController> _logger;
		private readonly IMediator _mediator;

        public CategoryController(ILogger<CategoryController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(){
            var result = await _mediator.Send(new GetCategoryList.Request());

            if (result != null){
                return Ok();
            }

            return UnprocessableEntity();
        }
    }
    }