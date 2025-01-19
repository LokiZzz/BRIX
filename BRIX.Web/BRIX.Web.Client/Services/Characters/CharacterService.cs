using BRIX.Library.Characters;
using BRIX.Web.Client.Extensions;
using BRIX.Web.Client.Models.Common;
using BRIX.Web.Client.Options;
using BRIX.Web.Client.Services.Http;
using BRIX.Web.Client.Services.UI;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace BRIX.Web.Client.Services.Characters
{
    public class CharacterManager
    {
        private readonly HttpClient _http;
        private readonly ModalService _modalService;

        public CharacterManager(
            HttpClient httpClient,
            IOptions<GameServiceOptions> gameServiceOptions,
            ModalService modalService)
        {
            _http = httpClient;
            _modalService = modalService;
            _http.BaseAddress = new Uri(gameServiceOptions.Value.ServiceAddress);
        }

        public Character? EditingCharacter { get; set; }

        public async Task<List<Character>> GetAllAsync()
        {
            _modalService.IsBusy = true;
            JsonResponse<List<Character>> response = await _http.GetJsonAsync<List<Character>>("api/character");
            _modalService.IsBusy = false;

            OperationResult result = response.ToOperationResult();

            if(result.Successfull)
            {
                return response.Payload ?? [];
            }

            _modalService.PushErrors(result.Errors);

            return [];
        }

        public async Task<Character?> GetAsync(Guid id)
        {
            _modalService.IsBusy = true;
            string uri = QueryHelpers.AddQueryString("api/character", "id", id.ToString());
            JsonResponse<List<Character>> response = await _http.GetJsonAsync<List<Character>>(uri);
            _modalService.IsBusy = false;

            OperationResult result = response.ToOperationResult();

            if (result.Successfull)
            {
                return response.Payload?.Single();
            }

            _modalService.PushErrors(result.Errors);

            return null;
        }

        public async Task<OperationResult> SaveAsync(Character character)
        {
            _modalService.IsBusy = true;

            JsonResponse response = await _http.PutJsonAsync("api/character", character);

            OperationResult result = response.ToOperationResult();

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return result;
        }

        public async Task<OperationResult> DeleteAsync(Character character)
        {
            _modalService.IsBusy = true;

            JsonResponse response = await _http.DeleteJsonAsync($"api/character?id={character.Id}");

            OperationResult result = response.ToOperationResult();

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return result;
        }
    }
}
