using LibraryAPI.DataHandling;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using LibraryAPI.Controllers;

namespace LibraryAPI.BusinessLogic
{
    public class MovieBL : ILibraryBL
    {
        public Queries dbHandler;
        public Utilities utils;
        public Posts posts;
        public ILogger log;

        public MovieBL(IConfiguration configuration, Queries _dbhandler, IHttpContextAccessor _context, ILogger<LogClass> _logger)
        {
            dbHandler = _dbhandler;
            utils = new Utilities();
            posts = new Posts(_context);
            log = _logger;
        }

        public List<Movie> GetAllMovies()
        {
            DataTable result = dbHandler.GetAllMovies();
            return utils.DataTableToList<Movie>(result);
        }

        public Movie GetMovieByID(int id)
        {
            DataRow result = dbHandler.GetMovieByID(id);
            return utils.DataRowToObject<Movie>(result);
        }

        public async Task<string> GetIMDBURL(string movie, IConfiguration configuration)
        {
            var ApiToken = configuration.GetValue<string>("MovieDBAPIApiToken");
            var PostURL = configuration.GetValue<string>("MovieDBBaseURL") + $"3/search/movie?&language=en-US&query={movie}&api_key={ApiToken}";

            var response = await posts.MakePostRequestAsync(PostURL, "","application/json",HttpMethod.Get);
            var MovieObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TheMovieDBResponse>(response);

            var ExternalMovieID = MovieObject?.results[0]?.id;
            PostURL = configuration.GetValue<string>("MovieDBBaseURL") + $"3/movie/{ExternalMovieID}/external_ids?api_key={ApiToken}";
            response = await posts.MakePostRequestAsync(PostURL, "", "application/json", HttpMethod.Get);

            var ExternalIMDBID = Newtonsoft.Json.JsonConvert.DeserializeObject<ExternalIDs>(response)?.imdb_id;
            var IMDBURL = configuration.GetValue<string>("IMDBBaseURL") + ExternalIMDBID;
            return IMDBURL;
        }

        public IActionResult InsertNewMovie(Movie movie)
        {
            int result = dbHandler.InsertNewMovie(movie);
            if (result > 0) 
            {
                return new ContentResult() { Content = "{\"Result\":\"Movie Inserted Successfully\"}", ContentType="application/json",StatusCode = 200};
            }
            else
            {
                return new ContentResult() { Content = "{\"Result\":\"Error While Inserting Movie\"}", ContentType = "application/json", StatusCode = 400 };
            }
        }

        public IActionResult DeleteMovie(int id)
        {
            int result = dbHandler.DeleteMovie(id);
            if (result >= 0)
            {
                return new ContentResult() { Content = "{\"Result\":\"Movie Deleted Successfully\"}", ContentType = "application/json", StatusCode = 200 };
            }
            else
            {
                return new ContentResult() { Content = "{\"Result\":\"Error While Deleting Movie\"}", ContentType = "application/json", StatusCode = 400 };
            }
        }
    }
}
