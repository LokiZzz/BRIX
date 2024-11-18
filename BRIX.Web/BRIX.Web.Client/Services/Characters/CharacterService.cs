﻿using BRIX.Library.Characters;
using BRIX.Web.Client.Services.Http;

namespace BRIX.Web.Client.Services.Characters
{
    public class CharacterManager(HttpClient http)
    {
        private readonly HttpClient _http = http;

        public Character EditingCharacter { get; set; } = default!;

        public async Task<List<Character>> GetAll()
        {
            return await _http.GetAsJsonAsync<List<Character>>("api/character/get");
        }
    }
}
