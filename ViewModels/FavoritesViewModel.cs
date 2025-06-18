using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace KinopoiskWpfApp.ViewModels
{
    public class FavoritesViewModel : ObservableObject
    {
        private readonly FavoritesService _favoritesService;
        private readonly FiltersCacheService _filtersCacheService;

        private ObservableCollection<Film> _allFavoriteFilms;
        private ObservableCollection<Film> _filteredFavoriteFilms;
        private ObservableCollection<Genre> _genres;
        private ObservableCollection<Country> _countries;

        private Genre _selectedGenre;
        private Country _selectedCountry;

        public ObservableCollection<Film> FilteredFavoriteFilms
        {
            get => _filteredFavoriteFilms;
            set => SetProperty(ref _filteredFavoriteFilms, value);
        }

        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set => SetProperty(ref _genres, value);
        }

        public ObservableCollection<Country> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                if (SetProperty(ref _selectedGenre, value))
                    ApplyFilters();
            }
        }

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (SetProperty(ref _selectedCountry, value))
                    ApplyFilters();
            }
        }

        public ICommand RemoveFromFavoritesCommand { get; }

        public FavoritesViewModel(FavoritesService favoritesService, FiltersCacheService filtersCacheService)
        {
            _favoritesService = favoritesService;
            _filtersCacheService = filtersCacheService;

            _allFavoriteFilms = new ObservableCollection<Film>(_favoritesService.GetFavorites());
            FilteredFavoriteFilms = new ObservableCollection<Film>(_allFavoriteFilms);

            Genres = new ObservableCollection<Genre>();
            Countries = new ObservableCollection<Country>();

            RemoveFromFavoritesCommand = new RelayCommand<Film>(RemoveFromFavorites);

            LoadFilters();
        }

        private void LoadFilters()
        {
            var filters = _filtersCacheService.Load();

            Genres.Clear();
            Genres.Add(new Genre { Id = 0, Name = "Все жанры" });
            foreach (var genre in filters.Genres
                                          .Where(g => !string.IsNullOrWhiteSpace(g.Name))
                                          .OrderBy(g => g.Name))
            {
                Genres.Add(genre);
            }

            Countries.Clear();
            Countries.Add(new Country { Id = 0, Name = "Все страны" });
            foreach (var country in filters.Countries
                                           .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                                           .OrderBy(c => c.Name))
            {
                Countries.Add(country);
            }

            SelectedGenre = Genres.First();
            SelectedCountry = Countries.First();
        }



        private void ApplyFilters()
        {
            var filtered = _allFavoriteFilms.AsEnumerable();

            if (SelectedGenre != null && SelectedGenre.Id != 0)
            {
                filtered = filtered.Where(f =>
                    f.Genres.Any(g => string.Equals(g.Name, SelectedGenre.Name, StringComparison.OrdinalIgnoreCase)));
            }

            if (SelectedCountry != null && SelectedCountry.Id != 0)
            {
                filtered = filtered.Where(f =>
                    f.Countries.Any(c => string.Equals(c.Name, SelectedCountry.Name, StringComparison.OrdinalIgnoreCase)));
            }

            FilteredFavoriteFilms = new ObservableCollection<Film>(filtered);
        }

        private void RemoveFromFavorites(Film film)
        {
            if (film == null) return;

            _favoritesService.RemoveFromFavorites(film);
            _allFavoriteFilms.Remove(film);
            FilteredFavoriteFilms.Remove(film);
        }
    }
}
