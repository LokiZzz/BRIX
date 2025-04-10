using BRIX.Library.Characters;

namespace BRIX.GameService.Services.Characters
{
    public interface ICharacterRepository
    {
        Task DeleteCharacterAsync(Guid characterId);
        Task<List<Character>> GetCharacterAsync(Guid userId, List<Guid>? characterIds = null);
        Task PushCharacterAsync(Guid userId, Character character);
        Task DeleteNPCAsync(Guid npcId);
        Task<List<NPC>> GetNPCAsync(Guid userId, List<Guid>? npcIds = null);
        Task PushNPCAsync(Guid userId, NPC npc);
    }
}