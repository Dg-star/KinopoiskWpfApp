using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KinopoiskWpfApp.ViewModels
{
    public class FavoritesViewModel : ObservableObject
    {
        private readonly FavoritesService _favoritesService;

        public ObservableCollection<Film> FavoriteFilms { get; }

        public ICommand RemoveFromFavoritesCommand { get; }

        public FavoritesViewModel(FavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
            FavoriteFilms = new ObservableCollection<Film>(_favoritesService.GetFavorites());

            RemoveFromFavoritesCommand = new RelayCommand<Film>(RemoveFromFavorites);
        }

        private void RemoveFromFavorites(Film film)
        {
            if (film == null) return;

            _favoritesService.RemoveFromFavorites(film);
            FavoriteFilms.Remove(film);
        }
    }
}
