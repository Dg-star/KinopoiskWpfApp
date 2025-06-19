using KinopoiskWpfApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace KinopoiskWpfApp.Services
{
    public class FilmsCacheService
    {
        private const string CacheFile = "films_cache.json";

        public List<Film> Load()
        {
            if (File.Exists(CacheFile))
            {
                var json = File.ReadAllText(CacheFile);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<List<Film>>(json);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public void Save(List<Film> films)
        {
            var json = JsonSerializer.Serialize(films, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CacheFile, json);
        }
    }
}
