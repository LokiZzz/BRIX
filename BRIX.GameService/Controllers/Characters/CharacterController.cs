using BRIX.GameService.Entities.Users;
using BRIX.GameService.Services.Account;
using BRIX.GameService.Services.Characters;
using BRIX.Library.Characters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BRIX.GameService.Controllers.Characters
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class CharacterController(
        IAccountService accountService,
        ICharacterRepository characterRepository) : Controller
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ICharacterRepository _characterRepository = characterRepository;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] List<Guid>? id)
        {
            User user = await _accountService.GetCurrentUserGuaranteed();
            List<Character> characters = await _characterRepository.Get(user.Id, id);

            return Ok(characters);
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
