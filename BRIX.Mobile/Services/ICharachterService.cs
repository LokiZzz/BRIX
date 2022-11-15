using BRIX.Library.Character;
using System.Text.Json;

namespace BRIX.Mobile.Services
{
    public interface ICharachterService
    {
        public Task<Character> CreateAsync(Character character);
        public Task<Character> GetAsync(Guid id);
        public Task<List<Character>> GetAllAsync();
        public Task RemoveAsync(Guid id);
        public Task RemoveAllAsync();
    }

    public class JsonCharacterService : ICharachterService
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonCharacterService(string path)
        {
            _path = path;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            if (!File.Exists(path))
            {
                string json = JsonSerializer.Serialize(new List<Character>(), _jsonOptions);
                File.WriteAllText(_path, json);
            }
        }

        public async Task<Character> CreateAsync(Character character)
        {
            List<Character> characters = await Read();
            character.Id = Guid.NewGuid();
            characters.Add(character);
            await Save(characters);
            return character;
        }

        private async Task<List<Character>> Read()
        {
            using (FileStream fs = File.Open(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                return await JsonSerializer.DeserializeAsync<List<Character>>(fs, _jsonOptions) ?? new List<Character>();
            }
        }

        private async Task Save(List<Character> characters)
        {
            string json = JsonSerializer.Serialize(characters, _jsonOptions);
            await File.WriteAllTextAsync(_path, json);
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await Read();
        }

        public async Task<Character> GetAsync(Guid id)
        {
            List<Character> characters = await Read();
            return characters.Single(charater => charater.Id == id);
        }

        public async Task RemoveAsync(Guid id)
        {
            List<Character> characters = await Read();
            Character character = characters.Single(character => character.Id == id);
            characters.Remove(character);
            await Save(characters);
        }

        public async Task RemoveAllAsync()
        {
            await Save(new List<Character>());
        }
    }
}
