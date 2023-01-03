namespace LibraryAPI.Models
{
    public class Result
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public IList<int> genre_ids { get; set; }
        public int id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class TheMovieDBResponse
    {
        public int page { get; set; }
        public IList<Result> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }

    public class ExternalIDs
    {
        public int id { get; set; }
        public string imdb_id { get; set; }
        public object wikidata_id { get; set; }
        public object facebook_id { get; set; }
        public object instagram_id { get; set; }
        public object twitter_id { get; set; }
    }

}
