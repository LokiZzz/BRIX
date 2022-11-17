using System.Text.Json;

namespace BRIX.Mobile.Services
{
    public interface ILocalStorage
    {
        string GetAppDataPath();
        bool FileExists(string fileName);
        Task WriteAllTextAsync(string fileName, string text);
        Task<string> ReadAllTextAsync(string fileName);
        Task<T> ReadJson<T>(string fileName, JsonSerializerOptions options = default) where T : class, new();
        Task<List<T>> ReadJsonCollectionAsync<T>(string fileName, JsonSerializerOptions options = default) where T : class, new();
        Task WriteJsonAsync<T>(string fileName, T collection, JsonSerializerOptions options = default) where T : class, new();
        Task WriteJsonCollectionAsync<T>(string fileName, List<T> collection, JsonSerializerOptions options = default) where T : class, new();
    }

    public class LocalStorage : ILocalStorage
    {
        public string GetAppDataPath() => FileSystem.AppDataDirectory;

        public bool FileExists(string fileName)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            return File.Exists(path);
        }

        public Task<string> ReadAllTextAsync(string fileName)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            return File.ReadAllTextAsync(path);
        }

        public Task WriteAllTextAsync(string fileName, string text)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            return File.WriteAllTextAsync(path, text);
        }

        public async Task<T> ReadJson<T>(string fileName, JsonSerializerOptions options = default) where T : class, new()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using (Stream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (fs.Length == 0)
                {
                    return default;
                }

                return await JsonSerializer.DeserializeAsync<T>(fs, options);
            }
        }

        public async Task<List<T>> ReadJsonCollectionAsync<T>(string fileName, JsonSerializerOptions options = default) where T : class, new()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using (Stream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (fs.Length == 0)
                {
                    return new List<T>();
                }

                return await JsonSerializer.DeserializeAsync<List<T>>(fs, options);
            }
        }

        public async Task WriteJsonAsync<T>(string path, T collection, JsonSerializerOptions options = default) where T : class, new()
        {
            string json = JsonSerializer.Serialize(collection, options);
            await WriteAllTextAsync(path, json);
        }

        public async Task WriteJsonCollectionAsync<T>(string path, List<T> collection, JsonSerializerOptions options = default) where T : class, new()
        {
            string json = JsonSerializer.Serialize(collection, options);
            await WriteAllTextAsync(path, json);
        }
    }
}
