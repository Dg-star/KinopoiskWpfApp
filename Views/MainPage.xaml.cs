using KinopoiskWpfApp.Services;
using KinopoiskWpfApp.ViewModels;
using System.Windows.Controls;

namespace KinopoiskWpfApp.Views
{
    public partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; }

        public MainPage()
        {
            InitializeComponent();

            ViewModel = new MainViewModel(new KinopoiskService());
            DataContext = ViewModel;
        }
    }
}
