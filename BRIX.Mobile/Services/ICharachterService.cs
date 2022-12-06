using BRIX.Library.Characters;
using System.Text.Json;

namespace BRIX.Mobile.Services
{
    public interface ICharacterService
    {
        public Task<Character> GetCurrentCharacter();
        public Task SelectCurrentCharacter(Character character);
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
        private readonly string _fileName;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonCharacterService(ILocalStorage storage)
        {
            _storage = storage;
            _fileName = "Characters.txt";
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
        }

        public async Task<Character> AddAsync(Character character)
        {
            List<Character> characters = await _storage.ReadJsonCollectionAsync<Character>(_fileName, _jsonOptions);
            character.Id = Guid.NewGuid();
            characters.Add(character);
            await _storage.WriteJsonCollectionAsync(_fileName, characters, _jsonOptions);

            return character;
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await _storage.ReadJsonCollectionAsync<Character>(_fileName, _jsonOptions);
        }

        public async Task<Character> GetAsync(Guid id)
        {
            List<Character> characters = await _storage.ReadJsonCollectionAsync<Character>(_fileName, _jsonOptions);

            return characters.Single(charater => charater.Id == id);
        }

        public async Task RemoveAsync(Guid id)
        {
            List<Character> characters = await _storage.ReadJsonCollectionAsync<Character>(_fileName, _jsonOptions);
            Character character = characters.Single(character => character.Id == id);
            characters.Remove(character);
            await _storage.WriteJsonCollectionAsync(_fileName, characters, _jsonOptions);
            
            if(!characters.Any())
            {
                await SelectCurrentCharacter(null);
            }
        }

        public async Task RemoveAllAsync()
        {
            await _storage.WriteJsonCollectionAsync(_fileName, new List<Character>(), _jsonOptions);
        }

        public async Task<Character> UpdateAsync(Character character)
        {
            await RemoveAsync(character.Id);
            await AddAsync(character);

            return character;
        }

        private Character _currentCharacter;

        public async Task SelectCurrentCharacter(Character character)
        {
            await Task.Run(() => {
                _currentCharacter = character;
            });
        }

        public async Task<Character> GetCurrentCharacter()
        {
            return await Task.FromResult(_currentCharacter);
        }
    }
}
