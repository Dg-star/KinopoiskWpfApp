using KinopoiskWpfApp.Models;
using System;
using System.IO;
using System.Text.Json;

namespace KinopoiskWpfApp.Services
{
    public class FiltersCacheService
    {
        private const string CacheFile = "filters.json";

        public FiltersCache Load()
        {
            if (File.Exists(CacheFile))
            {
                var json = File.ReadAllText(CacheFile);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<FiltersCache>(json);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public void Save(FiltersCache filters)
        {
            var json = JsonSerializer.Serialize(filters, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CacheFile, json);
        }
    }
}
