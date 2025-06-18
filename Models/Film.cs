using Newtonsoft.Json;
using System.Collections.Generic;

namespace KinopoiskWpfApp.Models
{
    public class Film
    {
        [JsonProperty("kinopoiskId")]
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

    public class Genre
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("genre")]
        public string Name { get; set; }
    }

    public class Country
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("country")]
        public string Name { get; set; }
    }
}
