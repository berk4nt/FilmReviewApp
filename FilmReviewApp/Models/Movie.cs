using System;

namespace FilmReviewApp.Models
{
    /// <summary>
    /// Represents a movie entity in the database.
    /// </summary>
    public class Movie
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the movie.
        /// </summary>
        public int FilmID { get; set; }

        /// <summary>
        /// Gets or sets the title of the movie.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the genre of the movie.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the director of the movie.
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Gets or sets the release year of the movie.
        /// </summary>
        public int ReleaseYear { get; set; }

        /// <summary>
        /// Gets or sets the description of the movie.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the path to the movie poster.
        /// </summary>
        public string PosterPath { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Movie class.
        /// </summary>
        public Movie()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Movie class with specified parameters.
        /// </summary>
        public Movie(int filmID, string title, string genre, string director, int releaseYear, string description, string posterPath)
        {
            FilmID = filmID;
            Title = title;
            Genre = genre;
            Director = director;
            ReleaseYear = releaseYear;
            Description = description;
            PosterPath = posterPath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the movie.
        /// </summary>
        public override string ToString()
        {
            return Title;
        }

        #endregion
    }
}
