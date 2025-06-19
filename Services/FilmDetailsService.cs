using KinopoiskWpfApp.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KinopoiskWpfApp.Services
{
    public class FilmDetailsService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "e7534db3-388a-487b-bc0a-14ed9e1d4be5";

        public FilmDetailsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        }

        public async Task<FilmDetails> GetFilmDetailsAsync(Film baseFilm)
        {
            var url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/{baseFilm.FilmId}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Ошибка при загрузке деталей фильма: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            var details = JsonConvert.DeserializeObject<FilmDetails>(json);

            if (details == null)
                throw new Exception("Ошибка при парсинге деталей фильма");

            // Связываем с базовой моделью
            details.Film = baseFilm;

            return details;
        }
    }
}
