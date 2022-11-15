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
        Task WriteJsonCollectionAsync<T>(string path, List<T> collection, JsonSerializerOptions options = default) where T : class, new();
    }

    public class LocalStorage : ILocalStorage
    {
        public string GetAppDataPath() => FileSystem.AppDataDirectory;

        public Task<bool> FileExistsAsync(string path)
        {
            return FileSystem.AppPackageFileExistsAsync(path);
        }

        public async Task<string> ReadAllTextAsync(string path)
        {
            using (Stream fs = await FileSystem.OpenAppPackageFileAsync(path))
            {
                if (fs.Length == 0)
                {
                    return default;
                }

                using (StreamReader sr = new StreamReader(fs))
                {
                    return await sr.ReadToEndAsync();
                }
            }
        }

        public async Task WriteAllTextAsync(string path, string text)
        {
            using (Stream fs = await FileSystem.OpenAppPackageFileAsync(path))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                fs.SetLength(0);
                sw.Write(text);
            }
        }

        public async Task<T?> ReadJson<T>(string path, JsonSerializerOptions options = default) where T : class, new()
        {
            using (Stream fs = await FileSystem.OpenAppPackageFileAsync(path))
            {
                if (fs.Length == 0)
                {
                    return default;
                }

                return await JsonSerializer.DeserializeAsync<T>(fs, options);
            }
        }

        public async Task<List<T>> ReadJsonCollectionAsync<T>(string path, JsonSerializerOptions options = default) where T : class, new()
        {
            using (Stream fs = await FileSystem.OpenAppPackageFileAsync(path))
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
            using (Stream fs = await FileSystem.OpenAppPackageFileAsync(path))
            {
                string json = JsonSerializer.Serialize(collection, options);
                await WriteAllTextAsync(path, json);
            }
        }

        public async Task WriteJsonCollectionAsync<T>(string path, List<T> collection, JsonSerializerOptions options = default) where T : class, new()
        {
            using (Stream fs = await FileSystem.OpenAppPackageFileAsync(path))
            {
                string json = JsonSerializer.Serialize(collection, options);
                await WriteAllTextAsync(path, json);
            }
        }
    }
}
