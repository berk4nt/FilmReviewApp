using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using FilmReviewApp.Models;

namespace FilmReviewApp.Data
{
    /// <summary>
    /// Handles all database operations using SQLite and ADO.NET.
    /// Manages movie and review data persistence.
    /// </summary>
    public class DatabaseHelper
    {
        #region Fields

        private readonly string _connectionString;
        private readonly string _databasePath;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DatabaseHelper class.
        /// Creates database if it doesn't exist and initializes tables.
        /// </summary>
        public DatabaseHelper()
        {
            // Store database in Application Data folder for proper installation practices
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "FilmReviewApp");

            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            _databasePath = Path.Combine(appFolder, "films.db");
            _connectionString = $"Data Source={_databasePath};Version=3;";

            InitializeDatabase();
        }

        #endregion

        #region Database Initialization

        /// <summary>
        /// Initializes the database by creating it if needed and ensuring all tables exist.
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                if (!File.Exists(_databasePath))
                {
                    SQLiteConnection.CreateFile(_databasePath);
                }

                CreateTables();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error initializing database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates the Films and Reviews tables if they don't exist.
        /// </summary>
        private void CreateTables()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // Create Films table
                    string createFilmsTable = @"
                        CREATE TABLE IF NOT EXISTS Films (
                            FilmID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Genre TEXT,
                            Director TEXT,
                            ReleaseYear INTEGER,
                            Description TEXT,
                            PosterPath TEXT
                        );";

                    // Create Reviews table
                    string createReviewsTable = @"
                        CREATE TABLE IF NOT EXISTS Reviews (
                            ReviewID INTEGER PRIMARY KEY AUTOINCREMENT,
                            FilmID INTEGER NOT NULL,
                            UserName TEXT NOT NULL,
                            Rating INTEGER NOT NULL,
                            Comment TEXT,
                            ReviewDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                            FOREIGN KEY(FilmID) REFERENCES Films(FilmID) ON DELETE CASCADE
                        );";

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = createFilmsTable;
                        command.ExecuteNonQuery();

                        command.CommandText = createReviewsTable;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating database tables: {ex.Message}", ex);
            }
        }

        #endregion

        #region Movie Operations

        /// <summary>
        /// Inserts a new movie into the database.
        /// </summary>
        /// <param name="title">The movie title.</param>
        /// <param name="genre">The movie genre.</param>
        /// <param name="director">The movie director.</param>
        /// <param name="releaseYear">The release year.</param>
        /// <param name="description">The movie description.</param>
        /// <param name="posterPath">The path to the poster image.</param>
        /// <returns>The ID of the inserted movie.</returns>
        public int InsertMovie(string title, string genre, string director, int releaseYear, string description, string posterPath)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Films (Title, Genre, Director, ReleaseYear, Description, PosterPath)
                        VALUES (@title, @genre, @director, @releaseYear, @description, @posterPath);
                        SELECT last_insert_rowid();";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Use parameterized queries to prevent SQL injection
                        command.Parameters.AddWithValue("@title", title ?? string.Empty);
                        command.Parameters.AddWithValue("@genre", genre ?? string.Empty);
                        command.Parameters.AddWithValue("@director", director ?? string.Empty);
                        command.Parameters.AddWithValue("@releaseYear", releaseYear);
                        command.Parameters.AddWithValue("@description", description ?? string.Empty);
                        command.Parameters.AddWithValue("@posterPath", posterPath ?? string.Empty);

                        object result = command.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting movie: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing movie in the database.
        /// </summary>
        /// <param name="filmID">The ID of the movie to update.</param>
        /// <param name="title">The new title.</param>
        /// <param name="genre">The new genre.</param>
        /// <param name="director">The new director.</param>
        /// <param name="releaseYear">The new release year.</param>
        /// <param name="description">The new description.</param>
        /// <param name="posterPath">The new poster path.</param>
        public void UpdateMovie(int filmID, string title, string genre, string director, int releaseYear, string description, string posterPath)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE Films
                        SET Title = @title, Genre = @genre, Director = @director, 
                            ReleaseYear = @releaseYear, Description = @description, PosterPath = @posterPath
                        WHERE FilmID = @filmID;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@filmID", filmID);
                        command.Parameters.AddWithValue("@title", title ?? string.Empty);
                        command.Parameters.AddWithValue("@genre", genre ?? string.Empty);
                        command.Parameters.AddWithValue("@director", director ?? string.Empty);
                        command.Parameters.AddWithValue("@releaseYear", releaseYear);
                        command.Parameters.AddWithValue("@description", description ?? string.Empty);
                        command.Parameters.AddWithValue("@posterPath", posterPath ?? string.Empty);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating movie: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a movie and all its associated reviews from the database.
        /// </summary>
        /// <param name="filmID">The ID of the movie to delete.</param>
        public void DeleteMovie(int filmID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // Delete reviews first (cascade is set but explicit deletion is safer)
                    string deleteReviewsQuery = "DELETE FROM Reviews WHERE FilmID = @filmID;";
                    string deleteMovieQuery = "DELETE FROM Films WHERE FilmID = @filmID;";

                    using (SQLiteCommand command = new SQLiteCommand(deleteReviewsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@filmID", filmID);
                        command.ExecuteNonQuery();
                    }

                    using (SQLiteCommand command = new SQLiteCommand(deleteMovieQuery, connection))
                    {
                        command.Parameters.AddWithValue("@filmID", filmID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting movie: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves all movies from the database.
        /// </summary>
        /// <returns>A list of all movies.</returns>
        public List<Movie> GetAllMovies()
        {
            List<Movie> movies = new List<Movie>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT FilmID, Title, Genre, Director, ReleaseYear, Description, PosterPath FROM Films ORDER BY Title ASC;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                movies.Add(new Movie(
                                    Convert.ToInt32(reader["FilmID"]),
                                    reader["Title"].ToString(),
                                    reader["Genre"].ToString(),
                                    reader["Director"].ToString(),
                                    Convert.ToInt32(reader["ReleaseYear"]),
                                    reader["Description"].ToString(),
                                    reader["PosterPath"].ToString()
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving movies: {ex.Message}", ex);
            }

            return movies;
        }

        /// <summary>
        /// Retrieves a specific movie by ID.
        /// </summary>
        /// <param name="filmID">The ID of the movie to retrieve.</param>
        /// <returns>The movie object, or null if not found.</returns>
        public Movie GetMovieById(int filmID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT FilmID, Title, Genre, Director, ReleaseYear, Description, PosterPath FROM Films WHERE FilmID = @filmID;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@filmID", filmID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Movie(
                                    Convert.ToInt32(reader["FilmID"]),
                                    reader["Title"].ToString(),
                                    reader["Genre"].ToString(),
                                    reader["Director"].ToString(),
                                    Convert.ToInt32(reader["ReleaseYear"]),
                                    reader["Description"].ToString(),
                                    reader["PosterPath"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving movie: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Retrieves all movies filtered by search term and genre.
        /// </summary>
        /// <param name="searchTerm">The search term (searches in title and genre).</param>
        /// <param name="genre">The genre filter (empty string for no filter).</param>
        /// <returns>A filtered list of movies.</returns>
        public List<Movie> SearchMovies(string searchTerm, string genre = "")
        {
            List<Movie> movies = new List<Movie>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT FilmID, Title, Genre, Director, ReleaseYear, Description, PosterPath 
                        FROM Films 
                        WHERE (Title LIKE @searchTerm OR Genre LIKE @searchTerm)";

                    if (!string.IsNullOrEmpty(genre) && genre != "All")
                    {
                        query += " AND Genre = @genre";
                    }

                    query += " ORDER BY Title ASC;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                        if (!string.IsNullOrEmpty(genre) && genre != "All")
                        {
                            command.Parameters.AddWithValue("@genre", genre);
                        }

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                movies.Add(new Movie(
                                    Convert.ToInt32(reader["FilmID"]),
                                    reader["Title"].ToString(),
                                    reader["Genre"].ToString(),
                                    reader["Director"].ToString(),
                                    Convert.ToInt32(reader["ReleaseYear"]),
                                    reader["Description"].ToString(),
                                    reader["PosterPath"].ToString()
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching movies: {ex.Message}", ex);
            }

            return movies;
        }

        /// <summary>
        /// Gets all distinct genres from the database.
        /// </summary>
        /// <returns>A list of unique genres.</returns>
        public List<string> GetAllGenres()
        {
            List<string> genres = new List<string>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT DISTINCT Genre FROM Films WHERE Genre IS NOT NULL AND Genre != '' ORDER BY Genre ASC;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                genres.Add(reader["Genre"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving genres: {ex.Message}", ex);
            }

            return genres;
        }

        #endregion

        #region Review Operations

        /// <summary>
        /// Inserts a new review into the database.
        /// </summary>
        /// <param name="filmID">The ID of the film being reviewed.</param>
        /// <param name="userName">The name of the reviewer.</param>
        /// <param name="rating">The rating (1-5).</param>
        /// <param name="comment">The review comment.</param>
        /// <returns>The ID of the inserted review.</returns>
        public int InsertReview(int filmID, string userName, int rating, string comment)
        {
            try
            {
                // Validate rating
                if (rating < 1 || rating > 5)
                {
                    throw new ArgumentException("Rating must be between 1 and 5.");
                }

                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Reviews (FilmID, UserName, Rating, Comment, ReviewDate)
                        VALUES (@filmID, @userName, @rating, @comment, CURRENT_TIMESTAMP);
                        SELECT last_insert_rowid();";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@filmID", filmID);
                        command.Parameters.AddWithValue("@userName", userName ?? string.Empty);
                        command.Parameters.AddWithValue("@rating", rating);
                        command.Parameters.AddWithValue("@comment", comment ?? string.Empty);

                        object result = command.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting review: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves all reviews for a specific movie.
        /// </summary>
        /// <param name="filmID">The ID of the film.</param>
        /// <returns>A list of reviews for the movie.</returns>
        public List<Review> GetReviewsByFilmId(int filmID)
        {
            List<Review> reviews = new List<Review>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT ReviewID, FilmID, UserName, Rating, Comment, ReviewDate 
                        FROM Reviews 
                        WHERE FilmID = @filmID 
                        ORDER BY ReviewDate DESC;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@filmID", filmID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reviews.Add(new Review(
                                    Convert.ToInt32(reader["ReviewID"]),
                                    Convert.ToInt32(reader["FilmID"]),
                                    reader["UserName"].ToString(),
                                    Convert.ToInt32(reader["Rating"]),
                                    reader["Comment"].ToString(),
                                    Convert.ToDateTime(reader["ReviewDate"])
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving reviews: {ex.Message}", ex);
            }

            return reviews;
        }

        /// <summary>
        /// Calculates the average rating for a movie.
        /// </summary>
        /// <param name="filmID">The ID of the film.</param>
        /// <returns>The average rating (0-5), or 0 if no reviews exist.</returns>
        public double GetAverageRating(int filmID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT AVG(CAST(Rating AS FLOAT)) FROM Reviews WHERE FilmID = @filmID;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@filmID", filmID);

                        object result = command.ExecuteScalar();

                        if (result == null || result is DBNull)
                        {
                            return 0.0;
                        }

                        return Convert.ToDouble(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating average rating: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the count of reviews for a movie.
        /// </summary>
        /// <param name="filmID">The ID of the film.</param>
        /// <returns>The number of reviews for the movie.</returns>
        public int GetReviewCount(int filmID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Reviews WHERE FilmID = @filmID;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@filmID", filmID);

                        object result = command.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error counting reviews: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
