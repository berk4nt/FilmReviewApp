using System;

namespace FilmReviewApp.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        public int FilmID { get; set; }

        public string UserName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; }

        public Review()
        {
        }

        public Review(int reviewID, int filmID, string userName, int rating, string comment, DateTime reviewDate)
        {
            ReviewID = reviewID;
            FilmID = filmID;
            UserName = userName;
            Rating = rating;
            Comment = comment;
            ReviewDate = reviewDate;
        }

        public override string ToString()
        {
            return $"{UserName} - {Rating}/5 - {ReviewDate:yyyy-MM-dd}";
        }
    }
}
