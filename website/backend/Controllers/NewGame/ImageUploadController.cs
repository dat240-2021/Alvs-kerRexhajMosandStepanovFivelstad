using System;
using System.Threading.Tasks;
using backend.Controllers.NewGame.Dto;
using backend.Core.Domain.Images.Pipelines;
using Controllers.Authentication;
using Controllers.Generics;
using Domain.Authentication.Services;
using Domain.Image.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // using Microsoft.AspNetCore.Http.StatusCode;

namespace backend.Controllers.NewGame
{

    [ApiController]
    // [Route("[controller]")]
    [Route("/api/imageupload")]
    public class ImageUploadController : ApiBaseController
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authenticationService;

        public ImageUploadController(ILogger<CategoryController> logger, IMediator mediator, IAuthenticationService auth)
        {
            _logger = logger;
            _mediator = mediator;
            _authenticationService = auth;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ImageUploadDto image)
        {

            // Console.WriteLine(Request.Body);
            // Console.WriteLine(image);

            var user = await _authenticationService.GetCurrentUser();
            var result = await _mediator.Send(new AddUserImage.Request(image.ImageList, user.Id));

            if (result.Success)
            {
                return Ok();
            }
            return UnprocessableEntity();
        }
    }
}