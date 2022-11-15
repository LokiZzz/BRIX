using BRIX.Library.Character;
using System.Text.Json;

namespace BRIX.Mobile.Services
{
    public interface ICharacterService
    {
        public Task<Character> AddAsync(Character character);
        public Task<Character> GetAsync(Guid id);
        public Task<List<Character>> GetAllAsync();
        public Task<Character> UpdateAsync(Character character);
        public Task RemoveAsync(Guid id);
        public Task RemoveAllAsync();
    }

    public class JsonCharacterService : ICharacterService
    {
        private readonly ILocalStorage _storage;
        private readonly string _path;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonCharacterService(ILocalStorage storage)
        {
            _storage = storage;
            _path = storage.GetAppDataPath();
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                
            };
        }

        public async Task<Character> AddAsync(Character character)
        {
            List<Character> characters = await _storage.ReadJsonCollectionAsync<Character>(_path, _jsonOptions);
            character.Id = Guid.NewGuid();
            characters.Add(character);
            await _storage.WriteJsonCollectionAsync(_path, characters, _jsonOptions);

            return character;
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await _storage.ReadJsonCollectionAsync<Character>(_path, _jsonOptions);
        }

        public async Task<Character> GetAsync(Guid id)
        {
            List<Character> characters = await _storage.ReadJsonCollectionAsync<Character>(_path, _jsonOptions);

            return characters.Single(charater => charater.Id == id);
        }

        public async Task RemoveAsync(Guid id)
        {
            List<Character> characters = await _storage.ReadJsonCollectionAsync<Character>(_path, _jsonOptions);
            Character character = characters.Single(character => character.Id == id);
            characters.Remove(character);
            await _storage.WriteJsonCollectionAsync(_path, characters, _jsonOptions);
        }

        public async Task RemoveAllAsync()
        {
            await _storage.WriteJsonCollectionAsync(_path, new List<Character>(), _jsonOptions);
        }

        public async Task<Character> UpdateAsync(Character character)
        {
            await RemoveAsync(character.Id);
            await AddAsync(character);
            return character;
        }
    }
}
