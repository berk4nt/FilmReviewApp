using System;
using System.Collections.Generic;
using FilmReviewApp.Data;

namespace FilmReviewApp.Utilities
{
    /// <summary>
    /// Sample data initialization utility for testing and demonstration purposes.
    /// Populates the database with sample movies and reviews.
    /// </summary>
    public static class SampleDataInitializer
    {
        /// <summary>
        /// Initializes the database with sample movies and reviews.
        /// Only adds data if database is empty to avoid duplicates.
        /// </summary>
        public static void InitializeSampleData()
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();

                // Only initialize if database is empty
                List<Models.Movie> existingMovies = db.GetAllMovies();
                if (existingMovies.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine("Database already contains movies. Skipping sample data initialization.");
                    return;
                }

                // Add sample movies
                int movieId1 = db.InsertMovie(
                    title: "The Shawshank Redemption",
                    genre: "Drama",
                    director: "Frank Darabont",
                    releaseYear: 1994,
                    description: "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    posterPath: ""
                );

                int movieId2 = db.InsertMovie(
                    title: "The Godfather",
                    genre: "Crime",
                    director: "Francis Ford Coppola",
                    releaseYear: 1972,
                    description: "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his youngest son.",
                    posterPath: ""
                );

                int movieId3 = db.InsertMovie(
                    title: "Inception",
                    genre: "Sci-Fi",
                    director: "Christopher Nolan",
                    releaseYear: 2010,
                    description: "A skilled thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea.",
                    posterPath: ""
                );

                int movieId4 = db.InsertMovie(
                    title: "The Dark Knight",
                    genre: "Action",
                    director: "Christopher Nolan",
                    releaseYear: 2008,
                    description: "When the menace known as the Joker wreaks havoc and chaos on Gotham, Batman must accept one of the greatest challenges to fight injustice.",
                    posterPath: ""
                );

                int movieId5 = db.InsertMovie(
                    title: "Pulp Fiction",
                    genre: "Crime",
                    director: "Quentin Tarantino",
                    releaseYear: 1994,
                    description: "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption.",
                    posterPath: ""
                );

                // Add sample reviews for The Shawshank Redemption
                db.InsertReview(movieId1, "John Doe", 5, "Absolutely masterpiece! Best movie ever made. Highly recommended.");
                db.InsertReview(movieId1, "Jane Smith", 5, "Incredible storytelling and acting. Watched it multiple times.");
                db.InsertReview(movieId1, "Bob Johnson", 4, "Great movie with powerful message about hope and friendship.");

                // Add sample reviews for The Godfather
                db.InsertReview(movieId2, "Alice Williams", 5, "A classic that defined cinema. Perfect in every way.");
                db.InsertReview(movieId2, "Charlie Brown", 5, "Epic narrative with unforgettable characters. Masterpiece!");
                db.InsertReview(movieId2, "Diana Prince", 4, "Excellent but a bit lengthy. Still a must-watch.");

                // Add sample reviews for Inception
                db.InsertReview(movieId3, "Edward Norton", 5, "Mind-bending and innovative. Christopher Nolan is a genius.");
                db.InsertReview(movieId3, "Fiona Apple", 4, "Confusing at times but fascinating. Great cinematography.");
                db.InsertReview(movieId3, "George Miller", 5, "Perfect blend of action, mystery, and philosophy.");

                // Add sample reviews for The Dark Knight
                db.InsertReview(movieId4, "Helena Bonham Carter", 5, "Heath Ledger's performance is unforgettable. Best superhero movie ever.");
                db.InsertReview(movieId4, "Ian McKellen", 4, "Outstanding film with excellent action sequences.");

                // Add sample reviews for Pulp Fiction
                db.InsertReview(movieId5, "Jodie Foster", 5, "Brilliant non-linear storytelling. Tarantino at his best.");
                db.InsertReview(movieId5, "Kevin Smith", 4, "Iconic dialogue and memorable scenes throughout.");

                System.Diagnostics.Debug.WriteLine("Sample data initialized successfully!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing sample data: {ex.Message}");
            }
        }
    }
}
