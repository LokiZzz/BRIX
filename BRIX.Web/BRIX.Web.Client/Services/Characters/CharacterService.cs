using BRIX.GameService.Contracts.Characters;
using BRIX.Library.Characters;
using BRIX.Web.Client.Services.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace BRIX.Web.Client.Services.Characters
{
    public class CharacterManager(HttpClient http)
    {
        private readonly HttpClient _http = http;

        public Character? EditingCharacter { get; set; }

        public async Task<List<Character>> GetAllAsync()
        {
            JsonResponse<List<Character>> response = await _http.GetJsonAsync<List<Character>>("api/character");

            // Временно просто кидаю эксепшн, пока нет обвеса для красивой обработки ошибок,
            // которое предназначены для пользователя.
            return response.Payload ?? throw new Exception("Ошибка при получении списка персонажей.");
        }

        public async Task<Character?> GetAsync(Guid id)
        {
            string uri = QueryHelpers.AddQueryString("api/character", "id", id.ToString());
            JsonResponse<List<Character>> response = await _http.GetJsonAsync<List<Character>>(uri);

            return response.Payload?.FirstOrDefault(x => x.Id == id);
        }

        public async Task<CharacterOperationResponse> SaveAsync(Character character)
        {
            JsonResponse<CharacterOperationResponse> response = 
                await _http.PutJsonAsync<Character, CharacterOperationResponse>("api/character", character);

            // Временно просто кидаю эксепшн, пока нет обвеса для красивой обработки ошибок,
            // которое предназначены для пользователя.
            return response.Payload ?? throw new Exception("Ошибка при сохранении персонажа.");
        }

        public async Task<CharacterOperationResponse> DeleteAsync(Character character)
        {
            JsonResponse<CharacterOperationResponse> response = 
                await _http.DeleteJsonAsync<CharacterOperationResponse>($"api/character?id={character.Id}");

            // Временно просто кидаю эксепшн, пока нет обвеса для красивой обработки ошибок,
            // которое предназначены для пользователя.
            return response.Payload ?? throw new Exception("Ошибка при удалении персонажа."); ;
        }
    }
}
