using BRIX.Library.Abilities;
using BRIX.Library.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Services
{
    /// <summary>
    /// Хранение сущностей, не связанных с персонажем, таких как статусы.
    /// </summary>
    public interface IAssetsService
    {
        public Task<List<Status>> GetStatuses();

        /// <summary>
        /// Просто перезаписывает статусы.
        /// </summary>
        public Task SaveStatuses(List<Status> statusesToSave);
    }
    public class JsonAssetsService(ILocalStorage storage) : IAssetsService
    {
        private readonly ILocalStorage _storage = storage;
        private readonly string _statusesFileName = "Statuses.txt";

        public async Task<List<Status>> GetStatuses()
        {
            return await _storage.ReadJson<List<Status>>(_statusesFileName) ?? [];
        }

        public async Task SaveStatuses(List<Status> statusesToSave)
        {
            await _storage.WriteJsonAsync(_statusesFileName, statusesToSave);
        }
    }
}
