using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using KinopoiskWpfApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace KinopoiskWpfApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly KinopoiskService _kinopoiskService;
        private readonly FavoritesService _favoritesService;
        private readonly FiltersCacheService _filtersCacheService;
        private readonly FilmsCacheService _filmsCacheService;
        private readonly NavigationService _navigationService;

        private ObservableCollection<Film> _films = new ObservableCollection<Film>();
        private ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();
        private ObservableCollection<Country> _countries = new ObservableCollection<Country>();

        private bool _isLoading;
        private string _errorMessage;

        private Genre _selectedGenre;
        private Country _selectedCountry;

        private bool _isLoadingFilms = false;
        public ObservableCollection<Film> Films
        {
            get => _films;
            set => SetProperty(ref _films, value);
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
                    _ = LoadFilmsAsync();
            }
        }

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (SetProperty(ref _selectedCountry, value))
                    _ = LoadFilmsAsync();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public ICommand NavigateToFilmDetailsCommand { get; }
        public ICommand LoadFilmsCommand { get; }
        public ICommand AddToFavoritesCommand { get; }

        public MainViewModel(KinopoiskService kinopoiskService, FavoritesService favoritesService, FiltersCacheService filtersCacheService, FilmsCacheService filmsCacheService, NavigationService navigationService)
        {
            _kinopoiskService = kinopoiskService ?? throw new ArgumentNullException(nameof(kinopoiskService));
            _favoritesService = favoritesService ?? throw new ArgumentNullException(nameof(favoritesService));
            _filtersCacheService = filtersCacheService ?? throw new ArgumentNullException(nameof(filtersCacheService));
            _filmsCacheService = filmsCacheService ?? throw new ArgumentNullException(nameof(filmsCacheService));
            _navigationService = navigationService;
            NavigateToFilmDetailsCommand = new RelayCommand<Film>(film =>
            {
                if (film == null) return;

                var detailsVM = new FilmDetailsViewModel(_favoritesService, film);
                var detailsPage = new FilmDetailsPage(_favoritesService, film)
                {
                    DataContext = detailsVM
                };

                _navigationService.Navigate(detailsPage);
            });

            

            LoadFilmsCommand = new RelayCommand(async () => await LoadFilmsAsync());
            AddToFavoritesCommand = new RelayCommand<Film>(AddToFavorites);

            Films = new ObservableCollection<Film>();
            Genres = new ObservableCollection<Genre>();
            Countries = new ObservableCollection<Country>();

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadFiltersAsync();
            await LoadFilmsAsync();
        }

        private async Task LoadFiltersAsync()
        {
            try
            {
                FiltersCache filters = _filtersCacheService.Load();

                if (filters == null)
                {
                    filters = await _kinopoiskService.GetFiltersAsync();
                    _filtersCacheService.Save(filters);
                }

                Genres.Clear();
                Genres.Add(new Genre { Id = 0, Name = "Все жанры" });
                foreach (var genre in filters.Genres
                                             .Where(g => !string.IsNullOrWhiteSpace(g.Name))
                                             .OrderBy(g => g.Name))
                {
                    Genres.Add(new Genre { Id = genre.Id, Name = genre.Name });
                }

                Countries.Clear();
                Countries.Add(new Country { Id = 0, Name = "Все страны" });
                foreach (var country in filters.Countries
                                               .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                                               .OrderBy(c => c.Name))
                {
                    Countries.Add(new Country { Id = country.Id, Name = country.Name });
                }

                SelectedGenre = Genres.First();
                SelectedCountry = Countries.First();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки фильтров: {ex.Message}";
            }
        }


        public async Task LoadFilmsAsync()
        {
            if (_isLoadingFilms) return;

            try
            {
                _isLoadingFilms = true;
                ErrorMessage = null;
                IsLoading = true;
                Films.Clear();

                var cachedFilms = _filmsCacheService.Load();

                List<Film> films;

                if (cachedFilms == null || cachedFilms.Count == 0)
                {
                    films = await _kinopoiskService.GetTopFilmsAsync();

                    // Убираем дубли из API
                    films = films
                        .GroupBy(f => f.FilmId)
                        .Select(g => g.First())
                        .ToList();

                    _filmsCacheService.Save(films);
                }
                else
                {
                    films = cachedFilms
                        .GroupBy(f => f.FilmId)
                        .Select(g => g.First())
                        .ToList();
                }

                if (SelectedGenre != null && SelectedGenre.Id != 0)
                    films = films.Where(f => f.Genres.Any(g => string.Equals(g.Name, SelectedGenre.Name, StringComparison.OrdinalIgnoreCase))).ToList();

                if (SelectedCountry != null && SelectedCountry.Id != 0)
                    films = films.Where(f => f.Countries.Any(c => string.Equals(c.Name, SelectedCountry.Name, StringComparison.OrdinalIgnoreCase))).ToList();

                foreach (var film in films)
                    Films.Add(film);

                if (Films.Count == 0)
                    ErrorMessage = "Нет фильмов по выбранным фильтрам.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                _isLoadingFilms = false;
            }
        }

        private void AddToFavorites(Film film)
        {
            if (film == null) return;

            try
            {
                _favoritesService.AddToFavorites(film);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка добавления в избранное: {ex.Message}");
            }
        }
    }
}
