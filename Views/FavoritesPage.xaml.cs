using KinopoiskWpfApp.Services;
using KinopoiskWpfApp.ViewModels;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows;

namespace KinopoiskWpfApp.Views
{
    public partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            InitializeComponent();
            Loaded += FavoritesPage_Loaded;
        }

        private void FavoritesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                var favoritesService = new FavoritesService();
                var filtersCacheService = new FiltersCacheService();
                var navigationService = this.NavigationService;

                DataContext = new FavoritesViewModel(favoritesService, filtersCacheService, navigationService);
            }
        }
    }
}
