using LibraryAPI.BusinessLogic;
using LibraryAPI.DataHandling;
using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        public MovieBL movieBL;
        public ILogger log;
        IConfiguration configuration;

        public MovieController(IConfiguration _configuration, Queries dbhandler, IHttpContextAccessor _context, ILogger<LogClass> _logger)
        {
            movieBL = new MovieBL(_configuration, dbhandler, _context, _logger);
            configuration = _configuration;
            log = _logger;
        }

        [HttpGet]
        public List<Movie> GetAllMovies()
        {
            return movieBL.GetAllMovies();
        }

        // GET api/<MovieController>/5
        [HttpGet("{id}")]
        //[HttpGet()]
        public Movie Get(int id)
        {
            return movieBL.GetMovieByID(id);
        }

        [HttpGet("{genre}")]
        public List<Movie> GetTop5RatedMoviesByGenre(string genre)
        {
            log.LogInformation($"Getting Top 5 Rated Movies for Genre - {genre}");
            return movieBL.GetAllMovies().OrderByDescending(x => x.Rating).Where(x => x.Genre == genre).Take(5).ToList();
        }
               
        [HttpPost()]
        public async Task<Movie> GetMovieByName([FromBody] string name)
        {
            return await movieBL.GetMovieByName(name);
        }

        // POST api/<MovieController>
        [HttpPost]
        public IActionResult Post([FromBody] Movie value)
        {
            var result = movieBL.InsertNewMovie(value);
            return result;
        }


        [HttpPost]
        public async Task<IActionResult> GetIMDBURL([FromBody] string value)
        {
            var result = await movieBL.GetIMDBURL(value, configuration);
            return new ContentResult() { Content = result, ContentType = "text/html", StatusCode = 200 };
        }

        // PUT api/<MovieController>/5
        [HttpPost("{id}")]
        public void Update(int id, [FromBody] Movie value)
        {
        }

        // DELETE api/<MovieController>/5
        [HttpGet("{id}")]
        public IActionResult Delete(int id)
        {
            var result = movieBL.DeleteMovie(id);
            return result;
        }
    }
}
