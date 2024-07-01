using Newtonsoft.Json;

namespace BRIX.Mobile.Services
{
    public interface ILocalStorage
    {
        string GetAppDataPath();
        bool FileExists(string fileName);
        Task WriteAllTextAsync(string fileName, string text);
        void WriteAllText(string fileName, string text);
        Task<string> ReadAllTextAsync(string fileName);
        string ReadAllText(string fileName);
        Task<T?> ReadJson<T>(string fileName) where T : class, new();
        Task WriteJsonAsync<T>(string fileName, T collection) where T : class, new();
    }

    public class LocalStorage : ILocalStorage
    {
        private static JsonSerializerSettings Settings => new()
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

        public string ReadAllText(string fileName)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(path))
            {
                WriteAllText(path, string.Empty);
            }

            return File.ReadAllText(path);
        }

        public async Task<T?> ReadJson<T>(string fileName) where T : class, new()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(path))
            {
                await WriteAllTextAsync(path, string.Empty);
            }

            string json = await File.ReadAllTextAsync(path);

            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public Task WriteAllTextAsync(string fileName, string text)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

            return File.WriteAllTextAsync(path, text);
        }

        public void WriteAllText(string fileName, string text)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

            File.WriteAllText(path, text);
        }

        public async Task WriteJsonAsync<T>(string path, T collection) where T : class, new()
        {
            string json = JsonConvert.SerializeObject(collection, Settings);
            await WriteAllTextAsync(path, json);
        }
    }
}
