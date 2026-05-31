using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using FilmReviewApp.Models;

namespace FilmReviewApp.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        private readonly string _databasePath;

        public DatabaseHelper()
        {
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

        private void InitializeDatabase()
        {
            try
            {
                bool isNewDatabase = !File.Exists(_databasePath);

                if (isNewDatabase)
                {
                    SQLiteConnection.CreateFile(_databasePath);
                }

                CreateTables();

                if (isNewDatabase || GetMovieCount() == 0)
                {
                    SeedDefaultMovies();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error initializing database: {ex.Message}", ex);
            }
        }

        private void CreateTables()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

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

        private int GetMovieCount()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Films;";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        private void SeedDefaultMovies()
        {
            try
            {
                string assetsPath = Path.Combine(Application.StartupPath, "Assets");

                var defaultMovies = new[]
                {
                    new { Title = "Inception", Genre = "Bilim Kurgu", Director = "Christopher Nolan", Year = 2010, Description = "Rüya hırsızı Dom Cobb, kurbanlarının bilinçaltına girerek sırlarını çalar. Ancak bu sefer görevi fikir çalmak değil, bir fikir yerleştirmektir. Inception, zihin bükücü bir bilim kurgu başyapıtıdır.", Poster = "inception.jpg.jpg" },
                    new { Title = "Interstellar", Genre = "Bilim Kurgu", Director = "Christopher Nolan", Year = 2014, Description = "Dünya artık yaşanılmaz hale gelirken, bir grup kaşif insanlık için yeni bir yuva bulmak üzere bir solucan deliğinden geçerek uzayın derinliklerine yolculuk yapar. Epik bir uzay macerası.", Poster = "interstellar.jpg.jpg" },
                    new { Title = "The Matrix", Genre = "Aksiyon", Director = "Lana Wachowski, Lilly Wachowski", Year = 1999, Description = "Neo, gerçekliğin aslında makineler tarafından yaratılmış bir simülasyon olduğunu keşfeder. İnsanlığı kurtarmak için Matrix'e karşı savaşmaya başlar. Devrim yaratan bir bilim kurgu aksiyon filmi.", Poster = "matrix.jpg.jpg" },
                    new { Title = "Titanic", Genre = "Romantik", Director = "James Cameron", Year = 1997, Description = "1912'de batan efsanevi Titanic gemisinde, farklı sınıflardan iki genç arasında büyüyen tutkulu bir aşk hikayesi. Jack ve Rose'un unutulmaz aşkı, tarihin en büyük deniz felaketinin ortasında.", Poster = "titanic.jpg.jpg" },
                    new { Title = "Avatar", Genre = "Bilim Kurgu", Director = "James Cameron", Year = 2009, Description = "Eski denizci Jake Sully, Pandora gezegeninde Na'vi ırkıyla bağ kurar. İnsanlığın kaynakları sömürme planına karşı durarak, iki dünya arasında bir seçim yapmak zorunda kalır.", Poster = "avatar.jpg.jpg" }
                };

                foreach (var movie in defaultMovies)
                {
                    string posterPath = Path.Combine(assetsPath, movie.Poster);
                    InsertMovie(movie.Title, movie.Genre, movie.Director, movie.Year, movie.Description, posterPath);
                }

                SeedDefaultReviews();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error seeding default movies: {ex.Message}");
            }
        }

        private void SeedDefaultReviews()
        {
            try
            {
                InsertReview(1, "Ahmet", 5, "Nolan'ın en iyi filmlerinden biri. Rüya içinde rüya konsepti muhteşem işlenmiş. Kesinlikle tekrar tekrar izlenmeli!");
                InsertReview(1, "Elif", 4, "Görsel efektler ve senaryo harika. Sadece bazı sahneler biraz karmaşık olabiliyor ama genel olarak çok başarılı.");
                InsertReview(1, "Mehmet", 5, "Zihin bükücü bir başyapıt! Hans Zimmer'in müzikleri de filmi ayrı bir seviyeye taşıyor.");

                InsertReview(2, "Zeynep", 5, "Hayatımda izlediğim en etkileyici film. Cooper ve Murph arasındaki baba-kız ilişkisi göz yaşartıcı.");
                InsertReview(2, "Can", 4, "Bilimsel detaylar çok iyi araştırılmış. Kara delik sahnesi tek başına filmi izlemeye değer kılıyor.");
                InsertReview(2, "Ayşe", 5, "Uzay sahneleri nefes kesici. Finali ise tamamen duygusal bir yıkım. Masterpiece!");

                InsertReview(3, "Burak", 5, "Sinema tarihini değiştiren bir film. Bullet-time efekti devrim niteliğinde. Neo efsanedir!");
                InsertReview(3, "Selin", 4, "Felsefe ve aksiyon mükemmel harmanlanmış. 'Gerçeklik nedir?' sorusunu sormadan edemiyorsunuz.");

                InsertReview(4, "Deniz", 5, "Her izlediğimde ağlıyorum. Jack ve Rose'un aşkı zamansız. James Cameron harika iş çıkarmış.");
                InsertReview(4, "Merve", 4, "Görsel olarak muhteşem bir yapım. Geminin batma sahneleri çok gerçekçi. Klasikler arasında hak ettiği yerde.");
                InsertReview(4, "Ali", 3, "Güzel bir film ama biraz uzun buldum. Yine de romantik film sevenlere kesinlikle tavsiye ederim.");

                InsertReview(5, "Emre", 4, "Pandora gezegeni inanılmaz detaylı tasarlanmış. 3D deneyimi sinema tarihinde bir dönüm noktası.");
                InsertReview(5, "Fatma", 5, "Görsel bir şölen! Na'vi kültürü ve doğayla bağ kurma teması çok güzel işlenmiş.");
                InsertReview(5, "Kaan", 4, "Hikaye biraz tahmin edilebilir olsa da görsel efektler ve dünya yaratımı eşsiz. İkinci filmi de harika.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error seeding default reviews: {ex.Message}");
            }
        }

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

        public void DeleteMovie(int filmID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

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

                    if (!string.IsNullOrEmpty(genre) && genre != "All" && genre != "Tümü")
                    {
                        query += " AND Genre = @genre";
                    }

                    query += " ORDER BY Title ASC;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                        if (!string.IsNullOrEmpty(genre) && genre != "All" && genre != "Tümü")
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

        public int InsertReview(int filmID, string userName, int rating, string comment)
        {
            try
            {
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
    }
}
