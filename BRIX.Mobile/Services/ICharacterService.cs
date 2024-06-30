using BRIX.Library.Characters;

namespace BRIX.Mobile.Services
{
    public interface ICharacterService
    {
        public Task<Character> GetCurrentCharacterGuaranteed();
        public Task<Character?> GetCurrentCharacter();
        public Task SelectCurrentCharacter(Character character);
        public Task<Character> AddAsync(Character character);
        public Task<Character> GetAsync(Guid id);
        public Task<List<Character>> GetAllAsync();
        public Task<Character> UpdateAsync(Character character);
        public Task RemoveAsync(Guid id);
        public Task RemoveAllAsync();

        // Временное решение для прототипа:
        public Task AddNPC(NPC npcToSave);
        public Task UpdateNPC(NPC npcToSave);
        public Task RemoveNPC(NPC npcToRemove);
        public Task<List<NPC>> GetNPCs();
    }

    public class JsonCharacterService(ILocalStorage storage) : ICharacterService
    {
        private readonly ILocalStorage _storage = storage;
        private readonly string _charactersFileName = "Characters.txt";
        private readonly string _npcsFileName = "NPCs.txt";

        public async Task AddNPC(NPC npcToSave)
        {
            List<NPC> npcs = await GetNPCs();
            npcs.Add(npcToSave);
            await _storage.WriteJsonAsync(_npcsFileName, npcs);
        }

        public async Task UpdateNPC(NPC npcToSave)
        {
            List<NPC> npcs = await GetNPCs();
            int index = npcs.IndexOf(npcs.First(x => x.Id == npcToSave.Id));
            npcs[index] = npcToSave;
            await _storage.WriteJsonAsync(_npcsFileName, npcs);
        }

        public async Task RemoveNPC(NPC npcToRemove)
        {
            List<NPC> npcs = await GetNPCs();
            npcs.Remove(npcs.First(x => x.Id == npcToRemove.Id));
            await _storage.WriteJsonAsync(_npcsFileName, npcs);
        }

        public async Task<List<NPC>> GetNPCs()
        {
            return await _storage.ReadJson<List<NPC>>(_npcsFileName) ?? [];
        }

        public async Task<Character> AddAsync(Character character)
        {
            List<Character> characters = await _storage.ReadJson<List<Character>>(_charactersFileName) ?? [];
            character.Id = Guid.NewGuid();
            characters.Add(character);
            await _storage.WriteJsonAsync(_charactersFileName, characters);

            return character;
        }

        public async Task<List<Character>> GetAllAsync()
        {
            List<Character> characters = await _storage.ReadJson<List<Character>>(_charactersFileName) ?? [];

            return characters;
        }

        public async Task<Character> GetAsync(Guid id)
        {
            List<Character> characters = await _storage.ReadJson<List<Character>>(_charactersFileName) ?? [];

            return characters.Single(charater => charater.Id == id);
        }

        public async Task RemoveAsync(Guid id)
        {
            List<Character> characters = await _storage.ReadJson<List<Character>>(_charactersFileName) ?? [];

            if (characters.Count != 0)
            {
                Character character = characters.Single(character => character.Id == id);
                characters.Remove(character);
                await _storage.WriteJsonAsync(_charactersFileName, characters);
            }

            if (characters.Count == 0)
            {
                await SelectCurrentCharacter(null);
            }
        }

        public async Task RemoveAllAsync()
        {
            await _storage.WriteJsonAsync(_charactersFileName, new List<Character>());
            await SelectCurrentCharacter(null);
        }

        public async Task<Character> UpdateAsync(Character character)
        {
            bool needToReselectCharacter = _currentCharacter != null && _currentCharacter.Id == character.Id;

            await RemoveAsync(character.Id);
            await AddAsync(character);

            if (needToReselectCharacter)
            {
                await SelectCurrentCharacter(character);
            }

            return character;
        }

        private Character? _currentCharacter = null;

        public async Task SelectCurrentCharacter(Character? character)
        {
            await Task.Run(() => {
                _currentCharacter = character;
            });
        }

        public async Task<Character?> GetCurrentCharacter()
        {
            return await Task.FromResult(_currentCharacter);
        }

        public async Task<Character> GetCurrentCharacterGuaranteed()
        {
            return await Task.FromResult(_currentCharacter)
                ?? throw new Exception("Character not found");
        }
    }
}
