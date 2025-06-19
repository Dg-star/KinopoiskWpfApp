using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;

namespace KinopoiskWpfApp.Models
{
    public class Film : ObservableObject
    {
        [JsonProperty("filmId")]
        public int FilmId { get; set; }

        [JsonProperty("nameRu")]
        public string NameRu { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("posterUrlPreview")]
        public string PosterUrlPreview { get; set; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; } = new List<Genre>();

        [JsonProperty("countries")]
        public List<Country> Countries { get; set; } = new List<Country>();

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }
    }

    public class FiltersCache
    {
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Country> Countries { get; set; } = new List<Country>();
    }

    public class Genre
    {
        // В API встречаются разные имена id, учитываем оба:
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("genreId")]
        private int? GenreId
        {
            set { if (value.HasValue) Id = value; }
        }

        [JsonProperty("genre")]
        public string Name { get; set; }
    }

    public class Country
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("countryId")]
        private int? CountryId
        {
            set { if (value.HasValue) Id = value; }
        }

        [JsonProperty("country")]
        public string Name { get; set; }
    }
}
