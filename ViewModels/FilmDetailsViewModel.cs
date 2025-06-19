using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;
using System;

namespace KinopoiskWpfApp.ViewModels
{
    public class FilmDetailsViewModel : ObservableObject
    {
        private readonly FavoritesService _favoritesService;

        public FilmDetailsViewModel(FavoritesService favoritesService, Film film)
        {
            _favoritesService = favoritesService ?? throw new ArgumentNullException(nameof(favoritesService));
            Film = film ?? throw new ArgumentNullException(nameof(film));

            AddToFavoritesCommand = new RelayCommand(AddToFavorites);
            OpenKinopoiskCommand = new RelayCommand(OpenKinopoisk);
        }

        public Film Film { get; }

        public string NameRu => Film.NameRu;
        public string PosterUrlPreview => Film.PosterUrlPreview;
        public string Year => Film.Year;
        public string GenresString => string.Join(", ", Film.Genres.Select(g => g.Name));
        public string CountriesString => string.Join(", ", Film.Countries.Select(c => c.Name));
        public string NameOriginal => Film?.NameOriginal ?? "";
        public string NameEn => Film?.NameEn ?? "";
        public string ShortDescription => Film?.ShortDescription ?? "";
        public double? RatingKinopoisk => Film?.RatingKinopoisk;
        public double? RatingImdb => Film?.RatingImdb;
        public double? RatingFilmCritics => Film?.RatingFilmCritics;
        public string Slogan => Film.Slogan;
        public string Description => Film.Description;
        public string ImdbId => Film.ImdbId;
        public string FilmLength => Film.FilmLength > 0 ? Film.FilmLength + " мин" : "";
        public string WebUrl => Film.WebUrl;
        public bool IsFavorite => Film?.IsFavorite ?? false;

        public ICommand AddToFavoritesCommand { get; }
        public ICommand OpenKinopoiskCommand { get; }

        private void AddToFavorites()
        {
            if (Film == null) return;

            try
            {
                _favoritesService.AddToFavorites(Film);
                Film.IsFavorite = true;
                OnPropertyChanged(nameof(IsFavorite));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка добавления в избранное: {ex.Message}");
            }
        }

        private void OpenKinopoisk()
        {
            if (!string.IsNullOrEmpty(Film.WebUrl))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(Film.WebUrl) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка открытия ссылки: {ex.Message}");
                }
            }
        }
    }
}