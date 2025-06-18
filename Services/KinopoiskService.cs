using KinopoiskWpfApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KinopoiskWpfApp.Services
{
    public class KinopoiskService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "7d3e436f-f59c-46dd-989d-dac71211f263";

        public KinopoiskService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        }

        public async Task<List<Film>> GetTopFilmsAsync()
        {
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/top?type=TOP_100_POPULAR_FILMS&page=1";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка запроса: {response.StatusCode}");
            }

            string jsonString = await response.Content.ReadAsStringAsync();

            var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);

            if (rootObject == null || rootObject.Films == null)
                throw new Exception("Ошибка парсинга данных с API");

            return rootObject.Films;
        }
    }

    // Модели для десериализации JSON (пример)
    public class RootObject
    {
        [JsonProperty("films")]
        public List<Film> Films { get; set; }
    }
}
