using KinopoiskWpfApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KinopoiskWpfApp.Services
{
    public class KinopoiskService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "e7534db3-388a-487b-bc0a-14ed9e1d4be5";

        public KinopoiskService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        }

        public async Task<List<Film>> GetTopFilmsAsync()
        {
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/top?type=TOP_100_POPULAR_FILMS&page=1";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Ошибка запроса: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);

            if (rootObject == null || rootObject.Films == null)
                throw new Exception("Ошибка парсинга данных с API");

            return rootObject.Films;
        }

        public async Task<FiltersCache> GetFiltersAsync()
        {
            var url = "https://kinopoiskapiunofficial.tech/api/v2.2/films/filters";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Ошибка запроса фильтров: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();

            var filters = JsonConvert.DeserializeObject<FiltersCache>(json);

            if (filters == null)
                throw new Exception("Ошибка парсинга фильтров");

            return filters;
        }
        public async Task<Film> GetFilmDetailsAsync(int filmId)
        {
            var url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/{filmId}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Ошибка запроса деталей фильма: {response.StatusCode}");

            var jsonString = await response.Content.ReadAsStringAsync();

            var film = JsonConvert.DeserializeObject<Film>(jsonString);

            if (film == null)
                throw new Exception("Ошибка парсинга деталей фильма");

            return film;
        }

    }

    public class RootObject
    {
        [JsonProperty("films")]
        public List<Film> Films { get; set; }
    }
}
