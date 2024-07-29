using Newtonsoft.Json;

namespace BRIX.Mobile.Services
{
    public interface ILocalStorage
    {
        void WriteText(string fileName, string text);
        Task WriteTextAsync(string fileName, string text);
        Task WriteJsonAsync<T>(string fileName, T collection) where T : class, new();
        string ReadText(string fileName);
        Task<T?> ReadJson<T>(string fileName) where T : class, new();
    }

    public class PreferencesStorage : ILocalStorage
    {
        private static JsonSerializerSettings Settings => new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
        };

        public Task<T?> ReadJson<T>(string fileName) where T : class, new()
        {
            string json = ReadText(fileName);

            return Task.FromResult(JsonConvert.DeserializeObject<T>(json, Settings));
        }

        public string ReadText(string fileName) => Preferences.Get(fileName, string.Empty);

        public Task WriteJsonAsync<T>(string fileName, T collection) where T : class, new()
        {
            string json = JsonConvert.SerializeObject(collection, Settings);
            WriteText(fileName, json);

            return Task.CompletedTask;
        }

        public void WriteText(string fileName, string text) => Preferences.Set(fileName, text);

        public Task WriteTextAsync(string fileName, string text)
        {
            WriteText(fileName, text);

            return Task.CompletedTask;
        }
    }

    //public class LocalStorage : ILocalStorage
    //{
    //    private static JsonSerializerSettings Settings => new()
    //    {
    //        Formatting = Formatting.Indented,
    //        TypeNameHandling = TypeNameHandling.All,
    //    };

    //    public string ReadText(string fileName)
    //    {
    //        string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

    //        if (!File.Exists(path))
    //        {
    //            WriteText(path, string.Empty);
    //        }

    //        return File.ReadAllText(path);
    //    }

    //    public async Task<T?> ReadJson<T>(string fileName) where T : class, new()
    //    {
    //        string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

    //        if (!File.Exists(path))
    //        {
    //            await WriteTextAsync(path, string.Empty);
    //        }

    //        string json = await File.ReadAllTextAsync(path);

    //        return JsonConvert.DeserializeObject<T>(json, Settings);
    //    }

    //    public Task WriteTextAsync(string fileName, string text)
    //    {
    //        string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

    //        return File.WriteAllTextAsync(path, text);
    //    }

    //    public void WriteText(string fileName, string text)
    //    {
    //        string path = Path.Combine(FileSystem.AppDataDirectory, fileName);

    //        File.WriteAllText(path, text);
    //    }

    //    public async Task WriteJsonAsync<T>(string path, T collection) where T : class, new()
    //    {
    //        string json = JsonConvert.SerializeObject(collection, Settings);
    //        await WriteTextAsync(path, json);
    //    }
    //}
}
