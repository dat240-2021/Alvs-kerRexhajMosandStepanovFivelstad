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
    [Route("/api/imageupload")]
    public class ImageUploadController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
		private readonly IMediator _mediator;

        public ImageUploadController(ILogger<CategoryController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ImageUploadDto image){
            var result = await _mediator.Send(new AddUserImage.Request(image.ImageList, image.UserId, image.ImageName));

            if (result.Success)
            {
                return Ok();
            }
            return UnprocessableEntity();
        }
    }
    }