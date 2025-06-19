using KinopoiskWpfApp.Services;
using KinopoiskWpfApp.ViewModels;
using KinopoiskWpfApp.Views;
using System.Windows;

namespace KinopoiskWpfApp
{
    public partial class MainWindow : Window
    {
        private readonly KinopoiskService _kinopoiskService = new KinopoiskService();
        private readonly FavoritesService _favoritesService = new FavoritesService();
        private readonly FiltersCacheService _filtersCacheService = new FiltersCacheService();
        private readonly FilmsCacheService _filmsCacheService = new FilmsCacheService();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => NavigateToMain();
        }

        private void NavigateToMain()
        {
            var vm = new MainViewModel(
                _kinopoiskService,
                _favoritesService,
                _filtersCacheService,
                _filmsCacheService,
                MainFrame.NavigationService);

            MainFrame.Navigate(new MainPage { DataContext = vm });
        }

        private void NavigateToFavorites()
        {
            var vm = new FavoritesViewModel(
                _favoritesService,
                _filtersCacheService,
                MainFrame.NavigationService);

            MainFrame.Navigate(new FavoritesPage { DataContext = vm });
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMain();
        }

        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToFavorites();
        }
    }
}
