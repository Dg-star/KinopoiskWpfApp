using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KinopoiskWpfApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly KinopoiskService _kinopoiskService;
        private readonly FavoritesService _favoritesService;

        private ObservableCollection<Film> _films = new ObservableCollection<Film>();
        private ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();
        private ObservableCollection<Country> _countries = new ObservableCollection<Country>();

        private bool _isLoading;
        private string _errorMessage;

        private Genre _selectedGenre;
        private Country _selectedCountry;

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
                {
                    _ = LoadFilmsAsync();
                }
            }
        }

        public Country SelectedCountry
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

        public MainViewModel(KinopoiskService kinopoiskService, FavoritesService favoritesService)
        {
            _kinopoiskService = kinopoiskService ?? throw new ArgumentNullException(nameof(kinopoiskService));
            _favoritesService = favoritesService ?? throw new ArgumentNullException(nameof(favoritesService));

            LoadFilmsCommand = new RelayCommand(async () => await LoadFilmsAsync());
            AddToFavoritesCommand = new RelayCommand<Film>(AddToFavorites);

            // Инициализация коллекций
            Films = new ObservableCollection<Film>();
            Genres = new ObservableCollection<Genre>();
            Countries = new ObservableCollection<Country>();

            // Загрузка фильтров и фильмов при старте
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
                Genres.Add(new Genre { Id = 0, Name = "Все жанры" });
                foreach (var genre in filters.Genres)
                    Genres.Add(genre);

                Countries.Clear();
                Countries.Add(new Country { Id = 0, Name = "Все страны" });
                foreach (var country in filters.Countries)
                    Countries.Add(country);

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
            try
            {
                ErrorMessage = null;
                IsLoading = true;
                Films.Clear();

                var films = await _kinopoiskService.GetTopFilmsAsync();

                if (SelectedGenre != null && SelectedGenre.Id != 0)
                    films = films.Where(f => f.Genres.Any(g => g.Id == SelectedGenre.Id)).ToList();

                if (SelectedCountry != null && SelectedCountry.Id != 0)
                    films = films.Where(f => f.Countries.Any(c => c.Id == SelectedCountry.Id)).ToList();

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
