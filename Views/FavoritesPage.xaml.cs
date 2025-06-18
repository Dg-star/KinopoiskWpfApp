using System.Windows.Controls;

namespace KinopoiskWpfApp.Views
{
    public partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            InitializeComponent();

            // Заглушка для избранного:
            FavoritesListBox.ItemsSource = new[]
            {
                "Избранный фильм 1",
                "Избранный фильм 2"
            };
        }

        private void GoBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }
    }
}
