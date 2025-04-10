﻿using BRIX.GameService.Entities.Users;
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
            List<Character> characters = await _characterRepository.GetCharacterAsync(user.Id, id);

            // TODO: УДАЛИТЬ ЭТО
            if((await _characterRepository.GetCharacterAsync(user.Id)).Count == 0)
            {
                await _characterRepository.PushCharacterAsync(user.Id, new Character { Name = "Siliel", Experience = 900 });
                await _characterRepository.PushCharacterAsync(user.Id, new Character { Name = "Loki", Experience = 1500 });
                await _characterRepository.PushCharacterAsync(user.Id, new Character { Name = "Boblin", Experience = 150 });
                characters = await _characterRepository.GetCharacterAsync(user.Id, id);
            }

            return Ok(characters);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Character character)
        {
            User user = await _accountService.GetCurrentUserGuaranteed();
            await _characterRepository.PushCharacterAsync(user.Id, character);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await _characterRepository.DeleteCharacterAsync(id);
            
            return Ok();
        }
    }
}
