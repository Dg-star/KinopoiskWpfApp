using KinopoiskWpfApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace KinopoiskWpfApp.Services
{
    public class FavoritesService
    {
        private const string FavoritesFileName = "favorites.json";

        private List<Film> _favorites = new List<Film>();

        public FavoritesService()
        {
            LoadFavorites();
        }

        private void LoadFavorites()
        {
            if (File.Exists(FavoritesFileName))
            {
                var json = File.ReadAllText(FavoritesFileName);
                _favorites = JsonSerializer.Deserialize<List<Film>>(json) ?? new List<Film>();
            }
        }

        private void SaveFavorites()
        {
            var json = JsonSerializer.Serialize(_favorites, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FavoritesFileName, json);
        }

        public IReadOnlyList<Film> GetFavorites() => _favorites.AsReadOnly();

        public void AddToFavorites(Film film)
        {
            if (film == null) return;

            if (!_favorites.Exists(f => f.FilmId == film.FilmId))
            {
                _favorites.Add(film);
                SaveFavorites();
            }
        }

        public void RemoveFromFavorites(Film film)
        {
            if (film == null) return;

            _favorites.RemoveAll(f => f.FilmId == film.FilmId);
            SaveFavorites();
        }

        public bool IsFavorite(Film film)
        {
            return film != null && _favorites.Exists(f => f.FilmId == film.FilmId);
        }
    }
}
