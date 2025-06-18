using KinopoiskWpfApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace KinopoiskWpfApp.Services
{
    public class FavoritesService
    {
        private const string FavoritesFileName = "favorites.json";

        private List<Film> _favorites = new List<Film>();

        // ObservableCollection для UI (уведомляет об изменениях)
        public ObservableCollection<Film> FavoriteFilms { get; private set; } = new ObservableCollection<Film>();

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
            else
            {
                _favorites = new List<Film>();
            }

            // Заполняем ObservableCollection
            FavoriteFilms.Clear();
            foreach (var film in _favorites)
            {
                FavoriteFilms.Add(film);
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

            if (film.FilmId > 0 && !_favorites.Exists(f => f.FilmId == film.FilmId))
            {
                _favorites.Add(film);
                SaveFavorites();
            }
        }

        public void RemoveFromFavorites(Film film)
        {
            if (film == null) return;

            _favorites.RemoveAll(f => f.FilmId == film.FilmId);
            var filmInCollection = FavoriteFilms.FirstOrDefault(f => f.FilmId == film.FilmId);
            if (filmInCollection != null)
            {
                FavoriteFilms.Remove(filmInCollection);
            }
            SaveFavorites();
        }

        public bool IsFavorite(Film film)
        {
            return film != null && _favorites.Exists(f => f.FilmId == film.FilmId);
        }
    }
}
