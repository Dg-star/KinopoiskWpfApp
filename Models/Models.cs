using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        [JsonProperty("kinopoiskId")]
        public int? KinopoiskId { get; set; }

        [JsonProperty("kinopoiskHDId")]
        public string KinopoiskHDId { get; set; }

        [JsonProperty("imdbId")]
        public string ImdbId { get; set; }

        [JsonProperty("nameEn")]
        public string NameEn { get; set; }

        [JsonProperty("nameOriginal")]
        public string NameOriginal { get; set; }

        [JsonProperty("posterUrl")]
        public string PosterUrl { get; set; }

        [JsonProperty("coverUrl")]
        public string CoverUrl { get; set; }

        [JsonProperty("logoUrl")]
        public string LogoUrl { get; set; }

        [JsonProperty("reviewsCount")]
        public int? ReviewsCount { get; set; }

        [JsonProperty("ratingGoodReview")]
        public double? RatingGoodReview { get; set; }

        [JsonProperty("ratingGoodReviewVoteCount")]
        public int? RatingGoodReviewVoteCount { get; set; }

        [JsonProperty("ratingKinopoisk")]
        public double? RatingKinopoisk { get; set; }

        [JsonProperty("ratingKinopoiskVoteCount")]
        public int? RatingKinopoiskVoteCount { get; set; }

        [JsonProperty("ratingImdb")]
        public double? RatingImdb { get; set; }

        [JsonProperty("ratingImdbVoteCount")]
        public int? RatingImdbVoteCount { get; set; }

        [JsonProperty("ratingFilmCritics")]
        public double? RatingFilmCritics { get; set; }

        [JsonProperty("ratingFilmCriticsVoteCount")]
        public int? RatingFilmCriticsVoteCount { get; set; }

        [JsonProperty("ratingAwait")]
        public double? RatingAwait { get; set; }

        [JsonProperty("ratingAwaitCount")]
        public int? RatingAwaitCount { get; set; }

        [JsonProperty("ratingRfCritics")]
        public double? RatingRfCritics { get; set; }

        [JsonProperty("ratingRfCriticsVoteCount")]
        public int? RatingRfCriticsVoteCount { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }

        [JsonProperty("filmLength")]
        public int? FilmLength { get; set; }

        [JsonProperty("slogan")]
        public string Slogan { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("editorAnnotation")]
        public string EditorAnnotation { get; set; }

        [JsonProperty("isTicketsAvailable")]
        public bool? IsTicketsAvailable { get; set; }

        [JsonProperty("productionStatus")]
        public string ProductionStatus { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("ratingMpaa")]
        public string RatingMpaa { get; set; }

        [JsonProperty("ratingAgeLimits")]
        public string RatingAgeLimits { get; set; }

        [JsonProperty("hasImax")]
        public bool? HasImax { get; set; }

        [JsonProperty("has3D")]
        public bool? Has3D { get; set; }

        [JsonProperty("lastSync")]
        public DateTime? LastSync { get; set; }

        [JsonProperty("startYear")]
        public int? StartYear { get; set; }

        [JsonProperty("endYear")]
        public int? EndYear { get; set; }

        [JsonProperty("serial")]
        public bool? Serial { get; set; }

        [JsonProperty("shortFilm")]
        public bool? ShortFilm { get; set; }

        [JsonProperty("completed")]
        public bool? Completed { get; set; }

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
