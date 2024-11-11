using BRIX.GameService.Entities;
using BRIX.GameService.Entities.Characters;
using BRIX.Library.Characters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BRIX.GameService.Services.Characters
{
    public class CharacterRepository(
        IDbContextFactory<ApplicationDbContext> contextFactory,
        JsonSerializerSettings jsonSerializerSettings) : ICharacterRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;
        private readonly JsonSerializerSettings _jsonSettings = jsonSerializerSettings;

        public async Task<List<Character>> Get(Guid userId, List<Guid>? characterIds = null)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            List<PlayerCharacter> playerCharacters = await context.PlayerCharacters
                .Where(x => x.UserId == userId && (characterIds == null || characterIds.Contains(x.Id)))
                .ToListAsync();

            List<Character> characters = playerCharacters
                .Select(x => JsonConvert.DeserializeObject<Character>(x.CharacterJsonData, _jsonSettings))
                .Where(x => x != null)
                .Cast<Character>()
                .ToList();

            return characters;
        }

        public async Task Push(Guid userId, Character character)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            string characterJson = JsonConvert.SerializeObject(character);

            if (character.Id != default)
            {
                PlayerCharacter? existingCharacter = context.PlayerCharacters
                    .FirstOrDefault(x => x.Id == character.Id);

                if (existingCharacter != null)
                {
                    existingCharacter.CharacterJsonData = characterJson;
                }
            }
            else
            {
                context.PlayerCharacters.Add(new PlayerCharacter
                {
                    UserId = userId,
                    CharacterJsonData = characterJson
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid characterId)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            PlayerCharacter? existingCharacter = context.PlayerCharacters.FirstOrDefault(x => x.Id == characterId);

            if (existingCharacter is not null)
            {
                context.PlayerCharacters.Remove(existingCharacter);
                await context.SaveChangesAsync();
            }
        }
    }
}
