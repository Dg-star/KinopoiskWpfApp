using Newtonsoft.Json;

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
    }
}
