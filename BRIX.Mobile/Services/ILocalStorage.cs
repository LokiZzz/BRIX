using System.Text.Json;

namespace BRIX.Mobile.Services
{
    public interface ILocalStorage
    {
        string GetAppDataPath();
        Task<bool> FileExistsAsync(string path);
        Task WriteAllTextAsync(string path, string text);
        Task<string> ReadAllTextAsync(string path);
        Task<T?> ReadJson<T>(string path, JsonSerializerOptions options = default) where T : class, new();
        Task<List<T>> ReadJsonCollectionAsync<T>(string path, JsonSerializerOptions options = default) where T : class, new();
        Task WriteJsonAsync<T>(string path, T collection, JsonSerializerOptions options = default) where T : class, new();
    }

    public class LocalStorage : ILocalStorage
    {
        public string GetAppDataPath() => FileSystem.AppDataDirectory;

        public Task<bool> FileExistsAsync(string path)
        {
            return Task.FromResult(File.Exists(path));
        }

        public Task<string> ReadAllTextAsync(string path)
        {
            return File.ReadAllTextAsync(path);
        }

        public Task WriteAllTextAsync(string path, string text)
        {
            return File.WriteAllTextAsync(path, text);
        }

        public async Task<T> ReadJson<T>(string path, JsonSerializerOptions options = default) where T : class, new()
        {
            string jsonData = await ReadAllTextAsync(path);

            return JsonSerializer.Deserialize<T>(jsonData, options);
        }

        public async Task<List<T>> ReadJsonCollectionAsync<T>(string path, JsonSerializerOptions options = default) where T : class, new()
        {
            string jsonData = await ReadAllTextAsync(path);

            return string.IsNullOrEmpty(jsonData) 
                ? JsonSerializer.Deserialize<List<T>>(jsonData, options)
                : new List<T>();
        }

        public async Task WriteJsonAsync<T>(string path, T collection, JsonSerializerOptions options = default) where T : class, new()
        {
            string json = JsonSerializer.Serialize(collection, options);
            await WriteAllTextAsync(path, json);
        }
    }
}
