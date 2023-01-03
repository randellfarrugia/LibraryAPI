using LibraryAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

namespace LibraryAPI.DataHandling
{
    public class Queries
    {
        public SQLConnection connection;
        public Queries(IConfiguration configuration, SQLConnection _sqlConnection)
        {
            connection = _sqlConnection;
        }

        public DataTable GetAllMovies()
        {
            StringBuilder query = new StringBuilder();
            List<KeyValuePair<string, object>> @params = new List<KeyValuePair<string, object>>();

            query.Append("SELECT * FROM Movie(NOLOCK);");
            return connection.ExecuteQuery<DataTable>(query.ToString(), @params);

        }

        public DataRow GetMovieByID(int id)
        {
            StringBuilder query = new StringBuilder();
            List<KeyValuePair<string, object>> @params = new List<KeyValuePair<string, object>>();

            query.Append("SELECT TOP(1)* FROM Movie(NOLOCK) WHERE ID=@ID;");
            @params.Add(new KeyValuePair<string, object>("@ID", id));
            return connection.ExecuteQuery<DataRow>(query.ToString(), @params);

        }

        public int InsertNewMovie(Movie movie)
        {
            StringBuilder query = new StringBuilder();
            List<KeyValuePair<string, object>> @params = new List<KeyValuePair<string, object>>();

            query.Append("INSERT INTO MOVIE (Name, Genre, Rating, YearReleased, Director) VALUES(@Name, @Genre, @Rating, @YearReleased, @Director)");

            @params.Add(new KeyValuePair<string, object>("@Name", movie.Name));
            @params.Add(new KeyValuePair<string, object>("@Genre", movie.Genre));
            @params.Add(new KeyValuePair<string, object>("@Rating", movie.Rating));
            @params.Add(new KeyValuePair<string, object>("@YearReleased", movie.YearReleased));
            @params.Add(new KeyValuePair<string, object>("@Director", movie.Director));

            return connection.ExecuteNonQuery(query.ToString(), @params, true);

        }

        public int DeleteMovie(int id)
        {
            StringBuilder query = new StringBuilder();
            List<KeyValuePair<string, object>> @params = new List<KeyValuePair<string, object>>();

            try
            {
                query.Append("DELETE FROM Movie WHERE ID=@ID;");
                @params.Add(new KeyValuePair<string, object>("@ID", id));
                connection.ExecuteNonQuery(query.ToString(), @params);
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 0;

        }

        public int UpdateMovie(Movie movie)
        {
            StringBuilder query = new StringBuilder();
            List<KeyValuePair<string, object>> @params = new List<KeyValuePair<string, object>>();

            query.Append("UPDATE MOVIE SET Name = @Name, Genre = @Genre, Rating = @Rating, YearReleased = @YearReleased, Director = @Director WHERE Id = @Id");

            @params.Add(new KeyValuePair<string, object>("@Name", movie.Name));
            @params.Add(new KeyValuePair<string, object>("@Genre", movie.Genre));
            @params.Add(new KeyValuePair<string, object>("@Rating", movie.Rating));
            @params.Add(new KeyValuePair<string, object>("@YearReleased", movie.YearReleased));
            @params.Add(new KeyValuePair<string, object>("@Director", movie.Director));
            @params.Add(new KeyValuePair<string, object>("@Id", movie.Id));

            var result = connection.ExecuteNonQuery(query.ToString(), @params);
           
            return result;
        }

    }
}
