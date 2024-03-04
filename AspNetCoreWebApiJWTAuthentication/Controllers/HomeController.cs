using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace AspNetCoreWebApiJWTAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            string name = HttpContext.User.Identity.Name;

            var claims = HttpContext.User.Claims;
            string username = claims.FirstOrDefault(u => u.Type == "name")?.Value;
            string email = claims.FirstOrDefault(u => u.Type == ClaimTypes.Email)?.Value;

            return Ok(new { success = true, name = name, username = username, email = email });
        }
    }
}
