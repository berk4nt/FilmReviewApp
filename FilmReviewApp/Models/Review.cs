using System;

namespace FilmReviewApp.Models
{
    /// <summary>
    /// Represents a review entity in the database.
    /// </summary>
    public class Review
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        public int ReviewID { get; set; }

        /// <summary>
        /// Gets or sets the film ID associated with this review.
        /// </summary>
        public int FilmID { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who wrote the review.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the rating (1-5) for the review.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the comment text of the review.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the date when the review was created.
        /// </summary>
        public DateTime ReviewDate { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Review class.
        /// </summary>
        public Review()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Review class with specified parameters.
        /// </summary>
        public Review(int reviewID, int filmID, string userName, int rating, string comment, DateTime reviewDate)
        {
            ReviewID = reviewID;
            FilmID = filmID;
            UserName = userName;
            Rating = rating;
            Comment = comment;
            ReviewDate = reviewDate;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the review.
        /// </summary>
        public override string ToString()
        {
            return $"{UserName} - {Rating}/5 - {ReviewDate:yyyy-MM-dd}";
        }

        #endregion
    }
}
