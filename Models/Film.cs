using KinopoiskWpfApp.Services;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KinopoiskWpfApp.Models
{
    public class Film
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
    }
}
