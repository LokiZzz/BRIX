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
            return await _http.GetAsJsonAsync<List<Character>>("api/character");
        }

        public async Task<CharacterOperationResponse> Save(Character character)
        {
            CharacterOperationResponse response = await _http.PutJsonAsync<Character, CharacterOperationResponse>(
                "api/character", 
                character
            );

            return response;
        }

        public async Task<CharacterOperationResponse> Delete(Character character)
        {
            CharacterOperationResponse response = await _http.DeleteJsonAsync<CharacterOperationResponse>(
                $"api/character?id={character.Id}"
            );

            return response;
        }
    }
}
