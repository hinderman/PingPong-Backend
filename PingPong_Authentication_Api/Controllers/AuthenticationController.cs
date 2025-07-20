using Microsoft.AspNetCore.Mvc;
using PingPong_Authentication_Api.Common;
using PingPong_Authentication_Application.Commands;
using PingPong_Authentication_Application.Queries;

namespace PingPong_Authentication_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiBaseController
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await Sender.Send(new GetById(id));

            return result.Match(r => Ok(r), e => Problem(e));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var result = await Sender.Send(login);

            return result.Match(r => Ok(r), e => Problem(e));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Create create)
        {
            var result = await Sender.Send(create);

            return result.Match(r => Ok(r), e => Problem(e));
        }

        [HttpPut]
        public async Task<IActionResult> Login([FromBody] Update update)
        {
            var result = await Sender.Send(update);

            return result.Match(r => Ok(r), e => Problem(e));
        }

        [HttpDelete]
        public async Task<IActionResult> Login([FromBody] Delete delete)
        {
            var result = await Sender.Send(delete);

            return result.Match(r => Ok(r), e => Problem(e));
        }
    }
}
