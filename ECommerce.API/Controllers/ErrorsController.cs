using AutoMapper.Configuration.Annotations;
using ECommerce.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult error (int code)
        {
            return NotFound(new ApiResponse (code));  
        }
    }
}
