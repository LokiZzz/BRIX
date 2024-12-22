using Azure;
using BRIX.GameService.Contracts.Characters;
using BRIX.Library.Characters;
using BRIX.Web.Client.Services.Http;

namespace BRIX.Web.Client.Services.Characters
{
    public class CharacterManager(HttpClient http)
    {
        private readonly HttpClient _http = http;

        public Character? EditingCharacter { get; set; }

        public async Task<List<Character>> GetAll()
        {
            JsonResponse<List<Character>> response = await _http.GetAsJsonAsync<List<Character>>("api/character");

            // Временно просто кидаю эксепшн, пока нет обвеса для красивой обработки ошибок,
            // которое предназначены для пользователя.
            return response.Payload ?? throw new Exception("Ошибка при получении списка персонажей.");
        }

        public async Task<CharacterOperationResponse> Save(Character character)
        {
            JsonResponse<CharacterOperationResponse> response = 
                await _http.PutJsonAsync<Character, CharacterOperationResponse>("api/character", character);

            // Временно просто кидаю эксепшн, пока нет обвеса для красивой обработки ошибок,
            // которое предназначены для пользователя.
            return response.Payload ?? throw new Exception("Ошибка при сохранении персонажа.");
        }

        public async Task<CharacterOperationResponse> Delete(Character character)
        {
            JsonResponse<CharacterOperationResponse> response = 
                await _http.DeleteJsonAsync<CharacterOperationResponse>($"api/character?id={character.Id}");

            // Временно просто кидаю эксепшн, пока нет обвеса для красивой обработки ошибок,
            // которое предназначены для пользователя.
            return response.Payload ?? throw new Exception("Ошибка при удалении персонажа."); ;
        }
    }
}
