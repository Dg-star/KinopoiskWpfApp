using KinopoiskWpfApp.Models;
using KinopoiskWpfApp.Services;
using KinopoiskWpfApp.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KinopoiskWpfApp.Views
{
    public partial class FilmDetailsPage : Page
    {
        private readonly KinopoiskService _kinopoiskService = new KinopoiskService();

        public FilmDetailsPage(FavoritesService favoritesService, Film basicFilm)
        {
            InitializeComponent();

            if (favoritesService == null)
                throw new ArgumentNullException(nameof(favoritesService));

            if (basicFilm == null)
                throw new ArgumentNullException(nameof(basicFilm));

            LoadFilmDetailsAsync(basicFilm, favoritesService);
        }

        private async void LoadFilmDetailsAsync(Film basicFilm, FavoritesService favoritesService)
        {
            try
            {
                var detailedFilm = await _kinopoiskService.GetFilmDetailsAsync(basicFilm.FilmId);
                DataContext = new FilmDetailsViewModel(favoritesService, detailedFilm);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить детали фильма: " + ex.Message);
                DataContext = new FilmDetailsViewModel(favoritesService, basicFilm);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
