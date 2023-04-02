using Newtonsoft.Json;

namespace BRIX.Mobile.Services
{
    public interface ILocalStorage
    {
        string GetAppDataPath();
        bool FileExists(string fileName);
        Task WriteAllTextAsync(string fileName, string text);
        Task<string> ReadAllTextAsync(string fileName);
        Task<T> ReadJson<T>(string fileName) where T : class, new();
        Task WriteJsonAsync<T>(string fileName, T collection) where T : class, new();
    }

    public class LocalStorage : ILocalStorage
    {
        private JsonSerializerSettings _settings => new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
        };

        public string GetAppDataPath() => FileSystem.AppDataDirectory;

        public bool FileExists(string fileName)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            return File.Exists(path);
        }

        public async Task<string> ReadAllTextAsync(string fileName)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(path))
            {
                await WriteAllTextAsync(path, string.Empty);
            }

            return await File.ReadAllTextAsync(path);
        }

        public async Task<T> ReadJson<T>(string fileName) where T : class, new()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(path))
            {
                await WriteAllTextAsync(path, string.Empty);
            }

            string json = await File.ReadAllTextAsync(path);

            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public Task WriteAllTextAsync(string fileName, string text)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

            return File.WriteAllTextAsync(path, text);
        }

        public async Task WriteJsonAsync<T>(string path, T collection) where T : class, new()
        {
            string json = JsonConvert.SerializeObject(collection, _settings);
            await WriteAllTextAsync(path, json);
        }
    }
}
