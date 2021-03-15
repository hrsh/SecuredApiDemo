using Microsoft.AspNetCore.Mvc;

namespace Is4.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DefaultController : ControllerBase
    {
        public DefaultController()
        {

        }

        public IActionResult Index() => Ok("IdentityServer4.Api");
    }
}
