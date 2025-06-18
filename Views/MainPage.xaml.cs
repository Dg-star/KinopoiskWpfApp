using KinopoiskWpfApp.ViewModels;
using KinopoiskWpfApp.Services;
using System.Windows.Controls;

namespace KinopoiskWpfApp.Views
{
    public partial class MainPage : Page
    {
        private MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            var kinopoiskService = new KinopoiskService();
            var favoritesService = new FavoritesService();
            var filtersCacheService = new FiltersCacheService();
            var filmsCacheService = new FilmsCacheService();
            _viewModel = new MainViewModel(kinopoiskService, favoritesService, filtersCacheService, filmsCacheService);

            this.DataContext = _viewModel;

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await _viewModel.LoadFilmsAsync();
        }
    }
}
