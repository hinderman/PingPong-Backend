using Microsoft.AspNetCore.Mvc;
using PingPong_ApiGateway_Api.Common;
using PingPong_ApiGateway_Application.Queries;

namespace PingPong_ApiGateway_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var result = await Sender.Send(login);

            return result.Match(r => Ok(r), e => Problem(e));
        }
    }
}
