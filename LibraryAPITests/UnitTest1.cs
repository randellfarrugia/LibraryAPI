using Castle.Core.Configuration;
//using Castle.Core.Logging;
using LibraryAPI;
using LibraryAPI.BusinessLogic;
using LibraryAPI.Controllers;
using LibraryAPI.DataHandling;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace LibraryAPITests
{
    public class MovieControllerTests
    {
        private IConfiguration _configurationMock;
        private Mock<SQLConnection> _sqlConnectionMock;
        private Mock<Queries> _dbHandlerMock;
        private Mock<IHttpContextAccessor> _contextMock;
        private Mock<ILogger<LogClass>> _logMock;
        private MovieController _movieController;
        private MovieBL _movieBL;
        private Utilities utils;




        [SetUp]
        public void Setup()
        {
           
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configurationMock = builder.Build();

            //var inMemorySettings = new Dictionary<string, string>
            //{
            //    {"MovieDBBaseURL", "https://api.themoviedb.org/"},
            //    {"MovieDBAPIApiToken", "66eb3bde9cca0487f03e78b512b451e4"},
            //    {"IMDBBaseURL", "https://www.imdb.com/title/"},
            //    {"ConnectionStrings:LibraryDB", "Data Source=DESKTOP-PL5OMP5;Initial Catalog=LibraryDatabase;User ID=sa;Password=root;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"},
            //};
            //_configurationMock = new ConfigurationBuilder()
            //    .AddInMemoryCollection(inMemorySettings)
            //    .Build();

            _logMock = new Mock<ILogger<LogClass>>();
            _sqlConnectionMock = new Mock<SQLConnection>(_configurationMock.GetConnectionString("LibraryDB"));
            _dbHandlerMock = new Mock<Queries>(_configurationMock, _sqlConnectionMock.Object);
            _contextMock = new Mock<IHttpContextAccessor>();
            _movieBL = new MovieBL(_configurationMock, _dbHandlerMock.Object, _contextMock.Object, _logMock.Object);
            _movieController = new MovieController(_configurationMock, _dbHandlerMock.Object, _contextMock.Object, _logMock.Object);
            utils = new Utilities();
        }

        [Test]
        public void GetAllMovies_ReturnsListOfMovies()
        {

            // Act
            var result = _movieController.GetAllMovies();

            // Assert
            Assert.IsInstanceOf<List<Movie>>(result);
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("Shawshank Redemption", result[0].Name);
            Assert.AreEqual("Drama", result[0].Genre);
        }

        [Test]

        public void GetTop5MoviesByGenre()
        {
            //Act

            var result = _movieController.GetTop5RatedMoviesByGenre("Drama");

            //Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<Movie>>(result);
            Assert.GreaterOrEqual(result.Count, 1);
            Assert.AreEqual(result[0].Genre, "Drama");
        }

        [Test]
        public void GetMovieByID()
        {
            //Act
            var response = _movieController.Get(3);

            //Assert
            Assert.NotNull(response);
            Assert.IsInstanceOf<Movie>(response);
            Assert.AreEqual(3, response.Id);
            Assert.AreEqual("Fight Club", response.Name);

        }

        [Test]
        public void GetIMDBURL()
        {
            //Act
            ContentResult _result = (ContentResult)_movieController.GetIMDBURL("Fight Club").Result;

            //Assert
            Assert.IsNotNull(_result.Content);
            Assert.IsInstanceOf<string>(_result.Content);
            Assert.AreEqual(200, _result.StatusCode);
            Assert.IsTrue(_result.Content.StartsWith("https"));
        }



    }
}
