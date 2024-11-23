﻿using BRIX.GameService.Contracts.Characters;
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
        ILogger<CharacterController> logger,
        IAccountService accountService,
        ICharacterRepository characterRepository) : Controller
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ICharacterRepository _characterRepository = characterRepository;
        private readonly ILogger<CharacterController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] List<Guid>? id)
        {
            User user = await _accountService.GetCurrentUserGuaranteed();
            List<Character> characters = await _characterRepository.Get(user.Id, id);

            // TODO: УДАЛИТЬ ЭТО
            if(characters.Count == 0)
            {
                await _characterRepository.Push(user.Id, new Character { Name = "Siliel", Experience = 900 });
                await _characterRepository.Push(user.Id, new Character { Name = "Loki", Experience = 1500 });
                await _characterRepository.Push(user.Id, new Character { Name = "Boblin", Experience = 150 });
                characters = await _characterRepository.Get(user.Id, id);
            }

            return Ok(characters);
        }

        [HttpPost]
        public async Task<IActionResult> Push([FromBody] Character character)
        {
            User user = await _accountService.GetCurrentUserGuaranteed();
            await _characterRepository.Push(user.Id, character);

            return Ok(new CharacterOperationResponse { Success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await _characterRepository.Delete(id);
            
            return Ok(new CharacterOperationResponse { Success = true });
        }
    }
}
