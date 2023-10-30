using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace RChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AuthenticationController : ControllerBase
    {

        public async Task<IActionResult> LoginAsync()
        {
            return null;
        }
        
        public async Task<IActionResult> RegisterAsync()
        {
            return null;
        }
    }
}
