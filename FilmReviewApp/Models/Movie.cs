using System;

namespace FilmReviewApp.Models
{
    public class Movie
    {
        public int FilmID { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string Director { get; set; }

        public int ReleaseYear { get; set; }

        public string Description { get; set; }

        public string PosterPath { get; set; }

        public Movie()
        {
        }

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

        public override string ToString()
        {
            return Title;
        }
    }
}
