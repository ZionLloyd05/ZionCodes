using Microsoft.AspNetCore.Mvc;

namespace ZionCodes.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() =>
            Ok("Hello Luigi, Mario has taken the princess to another castle!");
    }
}
