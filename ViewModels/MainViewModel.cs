using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

// Явные алиасы для устранения неоднозначности имён
using Models = KinopoiskWpfApp.Models;
using Services = KinopoiskWpfApp.Services;

namespace KinopoiskWpfApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly Services.KinopoiskService _kinopoiskService;
        private readonly Services.FavoritesService _favoritesService;

        private ObservableCollection<Models.Film> _films = new ObservableCollection<Models.Film>();
        private ObservableCollection<Models.Genre> _genres = new ObservableCollection<Models.Genre>();
        private ObservableCollection<Models.Country> _countries = new ObservableCollection<Models.Country>();

        private bool _isLoading;
        private string _errorMessage;

        private Models.Genre _selectedGenre;
        private Models.Country _selectedCountry;

        public ObservableCollection<Models.Film> Films
        {
            get => _films;
            set => SetProperty(ref _films, value);
        }

        public ObservableCollection<Models.Genre> Genres
        {
            get => _genres;
            set => SetProperty(ref _genres, value);
        }

        public ObservableCollection<Models.Country> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public Models.Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                if (SetProperty(ref _selectedGenre, value))
                {
                    _ = LoadFilmsAsync();
                }
            }
        }

        public Models.Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (SetProperty(ref _selectedCountry, value))
                {
                    _ = LoadFilmsAsync();
                }
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

        public ICommand LoadFilmsCommand { get; }
        public ICommand AddToFavoritesCommand { get; }

        public MainViewModel(Services.KinopoiskService kinopoiskService, Services.FavoritesService favoritesService)
        {
            _kinopoiskService = kinopoiskService ?? throw new ArgumentNullException(nameof(kinopoiskService));
            _favoritesService = favoritesService ?? throw new ArgumentNullException(nameof(favoritesService));

            LoadFilmsCommand = new RelayCommand(async () => await LoadFilmsAsync());
            AddToFavoritesCommand = new RelayCommand<Models.Film>(AddToFavorites);

            Films = new ObservableCollection<Models.Film>();
            Genres = new ObservableCollection<Models.Genre>();
            Countries = new ObservableCollection<Models.Country>();

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
                var filters = await _kinopoiskService.GetFiltersAsync();

                Genres.Clear();
                Genres.Add(new Models.Genre { Id = 0, Name = "Все жанры" });
                foreach (var genre in filters.Genres)
                    Genres.Add(new Models.Genre { Id = genre.Id, Name = genre.Name });

                Countries.Clear();
                Countries.Add(new Models.Country { Id = 0, Name = "Все страны" });
                foreach (var country in filters.Countries)
                    Countries.Add(new Models.Country { Id = country.Id, Name = country.Name });

                SelectedGenre = Genres.FirstOrDefault();
                SelectedCountry = Countries.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки фильтров: {ex.Message}";
            }
        }

        public async Task LoadFilmsAsync()
        {
            try
            {
                ErrorMessage = null;
                IsLoading = true;
                Films.Clear();

                var films = await _kinopoiskService.GetTopFilmsAsync();

                // Фильтрация по жанру
                if (SelectedGenre != null && SelectedGenre.Id != 0 && !string.IsNullOrWhiteSpace(SelectedGenre.Name))
                {
                    var genreName = SelectedGenre.Name.Trim().ToLowerInvariant();
                    films = films.Where(f => f.Genres.Any(g => !string.IsNullOrWhiteSpace(g.Name) && g.Name.Trim().ToLowerInvariant() == genreName)).ToList();
                }

                // Фильтрация по стране
                if (SelectedCountry != null && SelectedCountry.Id != 0 && !string.IsNullOrWhiteSpace(SelectedCountry.Name))
                {
                    var countryName = SelectedCountry.Name.Trim().ToLowerInvariant();
                    films = films.Where(f => f.Countries.Any(c => !string.IsNullOrWhiteSpace(c.Name) && c.Name.Trim().ToLowerInvariant() == countryName)).ToList();
                }

                foreach (var film in films)
                    Films.Add(film);

                if (Films.Count == 0)
                    ErrorMessage = "Нет фильмов по выбранным фильтрам.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки: {ex.Message}";
                Debug.WriteLine($"Ошибка: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddToFavorites(Models.Film film)
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
