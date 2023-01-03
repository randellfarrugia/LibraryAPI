using LibraryAPI.BusinessLogic;
using LibraryAPI.DataHandling;
using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace LibraryAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        public MovieBL movieBL;
        IConfiguration configuration;

        public MovieController(IConfiguration _configuration, Queries dbhandler, IHttpContextAccessor _context)
        {
            movieBL = new MovieBL(_configuration, dbhandler, _context);
            configuration = _configuration;
        }

        [HttpGet]
        public List<Movie> GetAllMovies()
        {
            return movieBL.GetAllMovies();
        }

        [HttpGet("{genre}")]
        public List<Movie> GetTop5RatedMoviesByGenre(string genre)
        {
            return movieBL.GetAllMovies().OrderByDescending(x => x.Rating).Where(x => x.Genre == genre).Take(5).ToList();
        }

        // GET api/<MovieController>/5
        [HttpGet("{id}")]
        //[HttpGet()]
        public Movie Get(int id)
        {
            return movieBL.GetMovieByID(id);
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
