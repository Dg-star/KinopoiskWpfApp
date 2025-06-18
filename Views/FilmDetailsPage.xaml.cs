using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using KinopoiskWpfApp.ViewModels;
using System;
using System.Windows.Controls;

namespace KinopoiskWpfApp.Views
{
    public partial class FilmDetailsPage : Page
    {
        public FilmDetailsPage(FavoritesService favoritesService, Film film)
        {
            InitializeComponent();

            if (favoritesService == null)
                throw new ArgumentNullException(nameof(favoritesService));

            if (film == null)
                throw new ArgumentNullException(nameof(film));

            DataContext = new FilmDetailsViewModel(favoritesService, film);
        }
    }
}
