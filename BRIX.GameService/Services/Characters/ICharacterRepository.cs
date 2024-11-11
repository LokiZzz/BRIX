using BRIX.Library.Characters;

namespace BRIX.GameService.Services.Characters
{
    public interface ICharacterRepository
    {
        Task Delete(Guid characterId);
        Task<List<Character>> Get(Guid userId, List<Guid>? characterIds = null);
        Task Push(Guid userId, Character character);
    }
}