using Microsoft.AspNetCore.Mvc;

namespace QueryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("from query");
    }
}