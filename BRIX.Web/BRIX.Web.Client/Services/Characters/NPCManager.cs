using BRIX.Library.Characters;
using BRIX.Utility.Extensions;
using BRIX.Web.Client.Extensions;
using BRIX.Web.Client.Models.Common;
using BRIX.Web.Client.Options;
using BRIX.Web.Client.Services.Http;
using BRIX.Web.Client.Services.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace BRIX.Web.Client.Services.Characters
{
    public class NPCManager
    {
        private readonly ModalService _modalService;
        private readonly NavigationManager _navigation;
        private readonly HttpClientBuilder _httpClientBuilder;

        private readonly string _baseAddress;

        public NPCManager(
            HttpClientBuilder httpClientBuilder,
            IOptions<GameServiceOptions> gameServiceOptions,
            ModalService modalService,
            NavigationManager navigation)
        {
            _httpClientBuilder = httpClientBuilder;
            _modalService = modalService;
            _navigation = navigation;
            _baseAddress = gameServiceOptions.Value.ServiceAddress;
        }

        /// <summary>
        /// Временное хранилище для изменяемого неигрового персонажа.
        /// </summary>
        public NPC? EditingNPC { get; private set; }

        public async Task<List<NPC>> GetAllAsync()
        {
            _modalService.IsBusy = true;
            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse<List<NPC>> response = await http.GetJsonAsync<List<NPC>>("api/npc");
            _modalService.IsBusy = false;

            OperationResult result = response.ToOperationResult();

            if(result.Successfull)
            {
                return response.Payload ?? [];
            }

            _modalService.PushErrors(result.Errors);

            return [];
        }

        public async Task<NPC?> GetAsync(Guid id)
        {
            _modalService.IsBusy = true;
            string uri = QueryHelpers.AddQueryString("api/npc", "id", id.ToString());
            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse<List<NPC>> response = await http.GetJsonAsync<List<NPC>>(uri);
            _modalService.IsBusy = false;

            OperationResult result = response.ToOperationResult();

            if (result.Successfull)
            {
                return response.Payload?.FirstOrDefault();
            }

            _modalService.PushErrors(result.Errors);

            return null;
        }

        public async Task<OperationResult> DeleteAsync(NPC npc)
        {
            _modalService.IsBusy = true;

            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse response = await http.DeleteJsonAsync($"api/npc?id={npc.Id}");

            OperationResult result = response.ToOperationResult();

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return result;
        }

        public async Task<OperationResult> SaveAsync(NPC npc)
        {
            _modalService.IsBusy = true;
            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse response = await http.PutJsonAsync("api/npc", npc);
            OperationResult result = response.ToOperationResult();

            if (result.Successfull)
            {
                Reset();
            }    

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return result;
        }

        /// <summary>
        /// Сохранить изменения, сделанные в EditingCharacter.
        /// </summary>
        public async Task<OperationResult> SaveAsync() => 
            await SaveAsync(EditingNPC ?? throw new Exception("No editing NPC."));

        /// <summary>
        /// Забрать персонажа с сервера и войти в режим редактирования.
        /// </summary>
        public async Task EditNPCAsync(Guid id)
        {
            NPC? npc = await GetAsync(id);

            if (npc is not null)
            {
                EditNPC(npc);
            }
        }

        /// <summary>
        /// Войти в режим редактирования. Персонаж копируется в свойство EditingCharacter и в любой момент
        /// изменения можно будет сбросить методов Reset, обнулив свойство и заново забрав персонажа с сервера.
        /// </summary>
        public void EditNPC(NPC npc)
        {
            if(EditingNPC is not null)
            {
                Reset();
            }

            EditingNPC = npc.Copy();
            _navigation.LocationChanged += ResetIfExitEditing;
        }

        public void Reset()
        {
            EditingNPC = null;
            _navigation.LocationChanged -= ResetIfExitEditing;
        }

        private void ResetIfExitEditing(object? sender, LocationChangedEventArgs e)
        {
            if (!e.Location.Contains("/abilities/"))
            {
                Reset();
            }
        }
    }
}