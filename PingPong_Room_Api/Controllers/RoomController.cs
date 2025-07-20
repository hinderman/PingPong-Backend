using Microsoft.AspNetCore.Mvc;
using PingPong_Room_Api.Common;
using PingPong_Room_Application.Queries;

namespace PingPong_Room_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Sender.Send(new GetAll());

            return result.Match(r => Ok(r), e => Problem(e));
        }
    }
}
