using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TokenGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        [HttpPost, Route("Login")]
        public async Task<IActionResult> TestAcc(TestRequest model)
        {
            return Ok("Hello");
        }
    }

    public class TestRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
