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
    [Route("api/[controller]")]
    public class NPCController(
        IAccountService accountService,
        ICharacterRepository characterRepository) : Controller
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ICharacterRepository _repository = characterRepository;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] List<Guid>? id)
        {
            User user = await _accountService.GetCurrentUserGuaranteed();
            List<NPC> npcs = await _repository.GetNPCAsync(user.Id, id);

            return Ok(npcs);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] NPC npc)
        {
            User user = await _accountService.GetCurrentUserGuaranteed();
            await _repository.PushNPCAsync(user.Id, npc);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await _repository.DeleteNPCAsync(id);

            return Ok();
        }
    }
}
