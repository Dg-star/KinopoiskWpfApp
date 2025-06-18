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
            DataContext = new FavoritesViewModel(new FavoritesService());
        }
    }
}
