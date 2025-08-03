using BRIX.GameService.Entities;
using BRIX.GameService.Entities.Characters;
using BRIX.Library.Characters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using NPC = BRIX.Library.Characters.NPC;
using NPCDao = BRIX.GameService.Entities.Characters.NPC;

namespace BRIX.GameService.Services.Characters
{
    public class CharacterRepository(
        IDbContextFactory<ApplicationDbContext> contextFactory,
        JsonSerializerSettings jsonSettings) : ICharacterRepository
    {
        public async Task<List<Character>> GetCharacterAsync(Guid userId, List<Guid>? characterIds = null)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            List<PlayerCharacter> playerCharacters = [];

            if (characterIds != null && characterIds.Count != 0)
            {
                playerCharacters = await context.PlayerCharacters
                    .Where(x => x.UserId == userId && characterIds.Contains(x.Id))
                    .ToListAsync();
            }
            else
            {
                playerCharacters = await context.PlayerCharacters
                    .Where(x => x.UserId == userId)
                    .ToListAsync();
            }

            List<Character> characters = [.. playerCharacters
                .Select(DeserializeCharacter)
                .Where(x => x != null)
                .Cast<Character>()];

            return characters;
        }    

        public async Task PushCharacterAsync(Guid userId, Character character)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            string json = JsonConvert.SerializeObject(character, jsonSettings);

            if (character.Id == default)
            {
                character.Id = Guid.NewGuid();
            }

            PlayerCharacter? existingCharacter = context.PlayerCharacters.FirstOrDefault(x => x.Id == character.Id);

            if (existingCharacter is not null)
            {
                existingCharacter.CharacterJsonData = json;
            }
            else
            {
                context.PlayerCharacters.Add(new PlayerCharacter
                {
                    Id = character.Id,
                    UserId = userId,
                    CharacterJsonData = json
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteCharacterAsync(Guid characterId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            PlayerCharacter? existingCharacter = context.PlayerCharacters.FirstOrDefault(x => x.Id == characterId);

            if (existingCharacter is not null)
            {
                context.PlayerCharacters.Remove(existingCharacter);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteNPCAsync(Guid npcId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            NPCDao? existingNPC = context.NPCs.FirstOrDefault(x => x.Id == npcId);

            if (existingNPC is not null)
            {
                context.NPCs.Remove(existingNPC);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<NPC>> GetNPCAsync(Guid userId, List<Guid>? npcIds = null)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            List<NPCDao> npcs = [];

            if (npcIds != null && npcIds.Count != 0)
            {
                npcs = await context.NPCs
                    .Where(x => x.UserId == userId && npcIds.Contains(x.Id))
                    .ToListAsync();
            }
            else
            {
                npcs = await context.NPCs
                    .Where(x => x.UserId == userId)
                    .ToListAsync();
            }

            List<NPC> characters = [.. npcs
                .Select(DeserializeNPC)
                .Where(x => x != null)
                .Cast<NPC>()];

            return characters;
        }

        public async Task PushNPCAsync(Guid userId, NPC npc)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            string json = JsonConvert.SerializeObject(npc, jsonSettings);

            if (npc.Id == default)
            {
                npc.Id = Guid.NewGuid();
            }

            NPCDao? existingNPC = context.NPCs.FirstOrDefault(x => x.Id == npc.Id);

            if (existingNPC is not null)
            {
                existingNPC.NPCJsonData = json;
            }
            else
            {
                context.NPCs.Add(new NPCDao
                {
                    Id = npc.Id,
                    UserId = userId,
                    NPCJsonData = json
                });
            }

            await context.SaveChangesAsync();
        }

        private Character? DeserializeCharacter(PlayerCharacter playerCharacter)
        {
            Character? character = JsonConvert.DeserializeObject<Character>(
                playerCharacter.CharacterJsonData, jsonSettings
            );

            if (character != null)
            {
                character.Id = playerCharacter.Id;
            }

            return character;
        }

        private NPC? DeserializeNPC(NPCDao npcDao)
        {
            NPC? npc = JsonConvert.DeserializeObject<NPC>(
                npcDao.NPCJsonData, jsonSettings
            );

            if (npc != null)
            {
                npc.Id = npcDao.Id;
            }

            return npc;
        }
    }
}
