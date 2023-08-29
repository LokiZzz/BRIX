using BRIX.Library.Ability;
using BRIX.Library.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Services
{
    /// <summary>
    /// Хранение сущностей, не связанных с персонажем, таких как статусы или монстры.
    /// </summary>
    public interface IAssetsService
    {
        public Task<List<Status>> GetStatuses();

        /// <summary>
        /// Просто перезаписывает статусы.
        /// </summary>
        public Task SaveStatuses(List<Status> statusesToSave);
    }
    public class JsonAssetsService : IAssetsService
    {
        private readonly ILocalStorage _storage;
        private readonly string _statusesFileName;

        public JsonAssetsService(ILocalStorage storage)
        {
            _storage = storage;
            _statusesFileName = "Statuses.txt";
        }

        public async Task<List<Status>> GetStatuses()
        {
            List<Status> statuses = await _storage.ReadJson<List<Status>>(_statusesFileName)
                ?? new List<Status>();

            return statuses;
        }

        public async Task SaveStatuses(List<Status> statusesToSave)
        {
            await _storage.WriteJsonAsync(_statusesFileName, statusesToSave);
        }
    }
}
