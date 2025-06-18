using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KinopoiskWpfApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly KinopoiskService _kinopoiskService;
        private readonly FavoritesService _favoritesService;

        private ObservableCollection<Film> _films;
        private bool _isLoading;
        private string _errorMessage;

        public ObservableCollection<Film> Films
        {
            get => _films;
            private set => SetProperty(ref _films, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadFilmsCommand { get; }
        public ICommand AddToFavoritesCommand { get; }

        public MainViewModel(KinopoiskService kinopoiskService, FavoritesService favoritesService)
        {
            _kinopoiskService = kinopoiskService ?? throw new ArgumentNullException(nameof(kinopoiskService));
            _favoritesService = favoritesService ?? throw new ArgumentNullException(nameof(favoritesService));


            Films = new ObservableCollection<Film>();

            AddToFavoritesCommand = new RelayCommand<Film>(AddToFavorites);
            LoadFilmsCommand = new RelayCommand(async () => await LoadFilmsAsync());
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

                Debug.WriteLine($"Получено фильмов: {films?.Count}");
                if (films != null && films.Count > 0)
                {
                    foreach (var film in films)
                    {
                        Films.Add(film);
                    }
                }
                else
                {
                    ErrorMessage = "Не удалось загрузить фильмы. Список пуст.";
                }
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
    }
}
