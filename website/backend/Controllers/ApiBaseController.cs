using Microsoft.AspNetCore.Mvc;

namespace Controllers.Generics
{
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        public IActionResult Ok<T>(T data)
        {
            return base.Ok(new GenericResponseObject<T>(data));
        }
        public IActionResult UnprocessableEntity<T>(T data)
        {
            return base.UnprocessableEntity(new GenericResponseObject<T>(data));
        }
    }
}