using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using System.Linq;
using System.Windows.Input;

namespace KinopoiskWpfApp.ViewModels
{
    public class FilmDetailsViewModel : ObservableObject
    {
        private readonly FavoritesService _favoritesService;

        public FilmDetailsViewModel(FavoritesService favoritesService, Film film)
        {
            _favoritesService = favoritesService;
            Film = film;

            AddToFavoritesCommand = new RelayCommand(AddToFavorites);
        }

        public Film Film { get; }

        public string NameRu => Film.NameRu;
        public string PosterUrlPreview => Film.PosterUrlPreview;
        public string Year => Film.Year;
        public string GenresString => string.Join(", ", Film.Genres.Select(g => g.Name));
        public string CountriesString => string.Join(", ", Film.Countries.Select(c => c.Name));

        public ICommand AddToFavoritesCommand { get; }

        private void AddToFavorites()
        {
            _favoritesService.AddToFavorites(Film);
        }
    }
}
