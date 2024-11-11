using BRIX.Library.Characters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BRIX.GameService.Controllers.Characters
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class CharacterController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid? id)
        {
            return Ok(new List<Character>());
        }

        [HttpPost]
        public async Task<IActionResult> Push([FromBody] Character character)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            return Ok();
        }
    }
}
