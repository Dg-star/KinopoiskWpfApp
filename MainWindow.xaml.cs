using KinopoiskWpfApp.Services;
using KinopoiskWpfApp.ViewModels;
using KinopoiskWpfApp.Views;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace KinopoiskWpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var kinopoiskService = new KinopoiskService();
            var favoritesService = new FavoritesService();
            var filtersCacheService = new FiltersCacheService();
            var filmsCacheService = new FilmsCacheService();

            var mainPage = new MainPage();
            MainFrame.Navigate(mainPage);

            var navigationService = MainFrame.NavigationService;

            var mainVM = new MainViewModel(
                kinopoiskService,
                favoritesService,
                filtersCacheService,
                filmsCacheService,
                navigationService);

            mainPage.DataContext = mainVM;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }

        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FavoritesPage());
        }
    }
}
