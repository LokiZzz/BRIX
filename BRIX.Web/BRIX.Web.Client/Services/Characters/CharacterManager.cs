﻿using BRIX.Library.Characters;
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
    public class CharacterManager
    {
        private readonly ModalService _modalService;
        private readonly NavigationManager _navigation;
        private readonly HttpClientBuilder _httpClientBuilder;

        private readonly string _baseAddress;

        public CharacterManager(
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
        /// Временное хранилище для изменяемого персонажа.
        /// </summary>
        public Character? EditingCharacter { get; private set; }

        public async Task<List<Character>> GetAllAsync()
        {
            _modalService.IsBusy = true;
            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse<List<Character>> response = await http.GetJsonAsync<List<Character>>("api/character");
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
            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse<List<Character>> response = await http.GetJsonAsync<List<Character>>(uri);
            _modalService.IsBusy = false;

            OperationResult result = response.ToOperationResult();

            if (result.Successfull)
            {
                return response.Payload?.FirstOrDefault();
            }

            _modalService.PushErrors(result.Errors);

            return null;
        }

        public async Task<OperationResult> DeleteAsync(Character character)
        {
            _modalService.IsBusy = true;

            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse response = await http.DeleteJsonAsync($"api/character?id={character.Id}");

            OperationResult result = response.ToOperationResult();

            _modalService.PushErrors(result.Errors);
            _modalService.IsBusy = false;

            return result;
        }

        public async Task<OperationResult> SaveAsync(Character character)
        {
            _modalService.IsBusy = true;
            using HttpClient http = await _httpClientBuilder.CreateAsync(_baseAddress);
            JsonResponse response = await http.PutJsonAsync("api/character", character);
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
            await SaveAsync(EditingCharacter ?? throw new Exception("No editing character."));

        /// <summary>
        /// Забрать персонажа с сервера и войти в режим редактирования.
        /// </summary>
        public async Task EditCharacterAsync(Guid id)
        {
            Character? character = await GetAsync(id);

            if (character is not null)
            {
                EditCharacter(character);
            }
        }

        /// <summary>
        /// Войти в режим редактирования. Персонаж копируется в свойство EditingCharacter и в любой момент
        /// изменения можно будет сбросить методов Reset, обнулив свойство и заново забрав персонажа с сервера.
        /// </summary>
        public void EditCharacter(Character characterToEdit)
        {
            if(EditingCharacter is not null)
            {
                Reset();
            }

            EditingCharacter = characterToEdit.Copy();
            _navigation.LocationChanged += ResetIfExitEditing;
        }

        public void Reset()
        {
            EditingCharacter = null;
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