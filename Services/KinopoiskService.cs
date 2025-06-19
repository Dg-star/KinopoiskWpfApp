using KinopoiskWpfApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KinopoiskWpfApp.Services
{
    public class KinopoiskService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "e7534db3-388a-487b-bc0a-14ed9e1d4be5";
        private const string BaseUrl = "https://kinopoiskapiunofficial.tech/api/v2.2/films";

        public KinopoiskService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<List<Film>> GetTopFilmsAsync()
        {
            var url = $"{BaseUrl}/top?type=TOP_100_POPULAR_FILMS&page=1";
            var result = await ExecuteApiRequestAsync<KinopoiskApiResponse>(url);
            return result?.Films ?? new List<Film>();
        }

        public async Task<FiltersCache> GetFiltersAsync()
        {
            var url = $"{BaseUrl}/filters";
            return await ExecuteApiRequestAsync<FiltersCache>(url);
        }

        public async Task<Film> GetFilmDetailsAsync(int filmId)
        {
            var url = $"{BaseUrl}/{filmId}";
            return await ExecuteApiRequestAsync<Film>(url);
        }

        private async Task<T> ExecuteApiRequestAsync<T>(string url) where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                var jsonString = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"API Request: {url}");
                Debug.WriteLine($"API Response ({response.StatusCode}): {jsonString}");

                if (!response.IsSuccessStatusCode)
                {
                    HandleApiError(response.StatusCode, jsonString);
                }

                if (IsHtmlResponse(jsonString))
                {
                    throw new KinopoiskApiException("Сервер вернул HTML вместо JSON. Возможно, проблема с API ключом.");
                }

                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    Error = (sender, args) =>
                    {
                        Debug.WriteLine($"JSON Parse Error: {args.ErrorContext.Error.Message}");
                        args.ErrorContext.Handled = true;
                    }
                };

                return JsonConvert.DeserializeObject<T>(jsonString, settings);
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine($"JSON Parsing Error: {ex.Message}");
                throw new KinopoiskApiException("Ошибка обработки данных от сервера", ex);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HTTP Request Error: {ex.Message}");
                throw new KinopoiskApiException("Ошибка подключения к серверу Kinopoisk", ex);
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"Request Timeout: {ex.Message}");
                throw new KinopoiskApiException("Превышено время ожидания ответа от сервера", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected Error: {ex.Message}");
                throw new KinopoiskApiException("Произошла непредвиденная ошибка", ex);
            }
        }

        private void HandleApiError(HttpStatusCode statusCode, string responseContent)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                throw new KinopoiskApiException("Неверный API ключ. Проверьте правильность ключа.");
            }

            if (statusCode == HttpStatusCode.NotFound)
            {
                throw new KinopoiskApiException("Запрошенный ресурс не найден.");
            }
            if ((int)statusCode == 429)
            {
                throw new KinopoiskApiException("Превышен лимит запросов. Попробуйте позже.");
            }

            try
            {
                var error = JsonConvert.DeserializeObject<KinopoiskError>(responseContent);
                throw new KinopoiskApiException(error?.Message ?? $"Ошибка API: {statusCode}");
            }
            catch
            {
                throw new KinopoiskApiException($"Ошибка API: {statusCode}\n{responseContent}");
            }
        }

        private bool IsHtmlResponse(string content)
        {
            return !string.IsNullOrEmpty(content) &&
                  (content.TrimStart().StartsWith("<!DOCTYPE html>") ||
                   content.Contains("<html>"));
        }
    }

    public class KinopoiskApiException : Exception
    {
        public KinopoiskApiException(string message) : base(message) { }
        public KinopoiskApiException(string message, Exception inner) : base(message, inner) { }
    }

    public class KinopoiskApiResponse
    {
        [JsonProperty("pagesCount")]
        public int PagesCount { get; set; }

        [JsonProperty("films")]
        public List<Film> Films { get; set; }
    }

    public class KinopoiskError
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}