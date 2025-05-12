using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mod3
{
    [XmlRoot("Movie")]
    public class Movie
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
    }

    [XmlRoot("Movies")]
    public class Movies : List<Movie>
    {
        public void AddMovie(Movie movie)
        {
            this.Add(movie);
        }
        public void RemoveMovie(Movie movie)
        {
            this.Remove(movie);
        }
    }

    public class MovieIO
    {
        public async Task<Movies> ReadMoviesAsync(string filePath)
        {
            Movies movies = null;
            XmlSerializer serializer = new XmlSerializer(typeof(Movies));

            // Open the file asynchronously
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                // Use the serializer to deserialize the XML asynchronously
                movies = (Movies)await Task.Run(() => serializer.Deserialize(fileStream));
            }

            return movies;
        }

        public async Task WriteMoviesAsync(string filePath, Movies movies)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Movies));
            // Open the file asynchronously
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                // Use the serializer to serialize the object asynchronously
                await Task.Run(() => serializer.Serialize(fileStream, movies));
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string binPath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(binPath, @"..\.."));
            string moviesPath = Path.Combine(projectRoot, "movies_ratings.xml");

            // Async read from file
            MovieIO movieIO = new MovieIO();
            Movies movies = await movieIO.ReadMoviesAsync(moviesPath);

            // Group by genre
            var groupedMoviesGenerator = from movie in movies
                                         group movie by movie.Genre into genreGroup
                                         select genreGroup;
            Console.WriteLine("Movies grouped by genre:");
            foreach (var genreGroup in groupedMoviesGenerator)
            {
                Console.WriteLine($"Genre: {genreGroup.Key}");
                foreach (var movie in genreGroup)
                {
                    Console.WriteLine($"  Title: {movie.Title}, Year: {movie.Year}, Rating: {movie.Rating}");
                }
            }
            Console.WriteLine("-------------------------------------------------\n");

            // Max average rating by genre
            var AverageRatingByGenre = from movie in movies
                                   group movie by movie.Genre into genreGroup
                                   let averageRating = genreGroup.Average(m => m.Rating)
                                   orderby averageRating descending
                                   select new { Genre = genreGroup.Key, AverageRating = averageRating };
            Console.WriteLine("Max average rating by genre:");
            var genre = AverageRatingByGenre.FirstOrDefault();
            Console.WriteLine($"  Genre: {genre.Genre}, Average Rating: {genre.AverageRating}");
            Console.WriteLine("-------------------------------------------------\n");

            // Movies with rating less than 6.0
            var lowRatedMovies = from movie in movies
                                 where movie.Rating < 6.0
                                 select movie;
            Console.WriteLine("Movies with rating less than 6.0:");
            foreach (var movie in lowRatedMovies)
            {
                Console.WriteLine($"  Title: {movie.Title}, Year: {movie.Year}, Rating: {movie.Rating}");
            }
            Console.WriteLine("-------------------------------------------------\n");

            // Grouped by genre and sorted by year
            var groupedMoviesSorted = from movie in movies
                                      group movie by movie.Genre into genreGroup
                                      select new
                                      {
                                          Genre = genreGroup.Key,
                                          Movies = genreGroup.OrderBy(m => m.Year)
                                      };

            Console.WriteLine("Movies grouped by genre and sorted by year:");
            foreach (var genreGroup in groupedMoviesSorted)
            {
                Console.WriteLine($"Genre: {genreGroup.Genre}");
                foreach (var movie in genreGroup.Movies)
                {
                    Console.WriteLine($"  Title: {movie.Title}, Year: {movie.Year}, Rating: {movie.Rating}");
                }
            }
            Console.WriteLine("-------------------------------------------------\n");

            // Top movies by rating for last 10 years
            var topMoviesLast10Years = from movie in movies
                                       where movie.Year >= DateTime.Now.Year - 10
                                       orderby movie.Rating descending
                                       select movie;
            var movies_tierlist = topMoviesLast10Years.Take(5).ToList();
            Console.WriteLine($"Top {movies_tierlist.Count()} movies from the last 10 years:");
            foreach (var movie in movies_tierlist)
            {
                Console.WriteLine($"  Title: {movie.Title}, Year: {movie.Year}, Rating: {movie.Rating}");
            }
            Console.WriteLine("-------------------------------------------------\n");

            // Put top movies with rating >= 8.5 to top_movies.xml
            var topMovies = from movie in movies
                            where movie.Rating >= 8.5
                            orderby movie.Rating descending
                            select movie;
            var topMoviesList = topMovies.ToList();
            string topMoviesPath = Path.Combine(projectRoot, "top_movies.xml");
            Movies topMoviesCollection = new Movies();
            foreach (var movie in topMoviesList)
            {
                topMoviesCollection.AddMovie(movie);
            }
            await movieIO.WriteMoviesAsync(topMoviesPath, topMoviesCollection);
            Console.WriteLine($"Top movies with rating >= 8.5 written to {topMoviesPath}");
            Console.WriteLine("-------------------------------------------------\n");

            // Rating < 6.0 movies to low_rated_movies.xml
            var lowRatedMoviesList = from movie in movies
                                     where movie.Rating < 6.0
                                     orderby movie.Rating ascending
                                     select movie;
            var lowRatedMoviesCollection = new Movies();
            foreach (var movie in lowRatedMoviesList)
            {
                lowRatedMoviesCollection.AddMovie(movie);
            }
            string lowRatedMoviesPath = Path.Combine(projectRoot, "low_rating_movies.xml");
            await movieIO.WriteMoviesAsync(lowRatedMoviesPath, lowRatedMoviesCollection);
            Console.WriteLine($"Low rated movies written to {lowRatedMoviesPath}");
            Console.WriteLine("-------------------------------------------------\n");

            // Grouped by ganre to file
            var groupedMovies = from movie in movies
                                group movie by movie.Genre into genreGroup
                                select new
                                {
                                    Genre = genreGroup.Key,
                                    Movies = genreGroup.ToList()
                                };
            string groupedMoviesPath = Path.Combine(projectRoot, "grouped_movies.xml");
            Movies groupedMoviesCollection = new Movies();
            foreach (var genreGroup in groupedMovies)
            {
                foreach (var movie in genreGroup.Movies)
                {
                    groupedMoviesCollection.AddMovie(movie);
                }
            }
            await movieIO.WriteMoviesAsync(groupedMoviesPath, groupedMoviesCollection);
            Console.WriteLine($"Grouped movies written to {groupedMoviesPath}");
            Console.WriteLine("-------------------------------------------------\n");

            // Count of movies by genre
            var movieCountByGenre = from movie in movies
                                    group movie by movie.Genre into genreGroup
                                    select new { Genre = genreGroup.Key, Count = genreGroup.Count() };
            Console.WriteLine("Count of movies by genre:");
            foreach (var genreGroup in movieCountByGenre)
            {
                Console.WriteLine($"  Genre: {genreGroup.Genre}, Count: {genreGroup.Count}");
            }
            Console.WriteLine("-------------------------------------------------\n");

            // Avg rating by genre
            var avgRatingByGenre = from movie in movies
                                   group movie by movie.Genre into genreGroup
                                   select new { Genre = genreGroup.Key, AvgRating = genreGroup.Average(m => m.Rating) };
            Console.WriteLine("Average rating by genre:");
            foreach (var genreGroup in avgRatingByGenre)
            {
                Console.WriteLine($"  Genre: {genreGroup.Genre}, Avg Rating: {genreGroup.AvgRating}");
            }
            Console.WriteLine("-------------------------------------------------");
        }
    }
}
