using KinopoiskWpfApp.Services;
using KinopoiskWpfApp.ViewModels;
using System.Windows.Controls;

namespace KinopoiskWpfApp.Views
{
    public partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            InitializeComponent();

            var favoritesService = new FavoritesService();
            var filtersCacheService = new FiltersCacheService();

            DataContext = new FavoritesViewModel(favoritesService, filtersCacheService);
        }
    }
}
