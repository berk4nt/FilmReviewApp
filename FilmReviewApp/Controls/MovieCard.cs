using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FilmReviewApp.Controls
{
    /// <summary>
    /// A reusable UserControl representing a movie card.
    /// Displays poster, title, genre, and rating in a Netflix/IMDb style card.
    /// </summary>
    public partial class MovieCard : UserControl
    {
        #region Fields

        private int _movieId;
        private string _title;
        private string _genre;
        private double _rating;
        private string _posterPath;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the movie ID.
        /// </summary>
        public int MovieId
        {
            get { return _movieId; }
            set { _movieId = value; }
        }

        /// <summary>
        /// Gets or sets the movie title.
        /// </summary>
        public string MovieTitle
        {
            get { return _title; }
            set
            {
                _title = value;
                labelTitle.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the movie genre.
        /// </summary>
        public string MovieGenre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                labelGenre.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the average rating of the movie (0-5).
        /// </summary>
        public double Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                UpdateRatingDisplay();
            }
        }

        /// <summary>
        /// Gets or sets the path to the movie poster image.
        /// </summary>
        public string PosterPath
        {
            get { return _posterPath; }
            set
            {
                _posterPath = value;
                LoadPoster();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the Details button is clicked.
        /// </summary>
        public event EventHandler DetailsClicked;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MovieCard control.
        /// </summary>
        public MovieCard()
        {
            SetupDesign();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Sets up the visual design of the movie card.
        /// </summary>
        private void SetupDesign()
        {
            // Dark theme colors
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;

            // Set border and shadow effect
            BorderStyle = BorderStyle.FixedSingle;

            // Initialize labels if not already done by designer
            if (labelTitle == null)
            {
                labelTitle = new Label();
                labelTitle.ForeColor = Color.White;
                labelTitle.BackColor = Color.FromArgb(30, 30, 30);
                labelTitle.AutoSize = false;
                labelTitle.TextAlign = ContentAlignment.TopLeft;
                Controls.Add(labelTitle);
            }

            if (labelGenre == null)
            {
                labelGenre = new Label();
                labelGenre.ForeColor = Color.Silver;
                labelGenre.BackColor = Color.FromArgb(30, 30, 30);
                labelGenre.AutoSize = false;
                Controls.Add(labelGenre);
            }

            if (labelRating == null)
            {
                labelRating = new Label();
                labelRating.ForeColor = Color.Gold;
                labelRating.BackColor = Color.FromArgb(30, 30, 30);
                labelRating.TextAlign = ContentAlignment.MiddleCenter;
                labelRating.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                Controls.Add(labelRating);
            }

            if (pictureBoxPoster == null)
            {
                pictureBoxPoster = new PictureBox();
                pictureBoxPoster.BackColor = Color.FromArgb(40, 40, 40);
                pictureBoxPoster.SizeMode = PictureBoxSizeMode.Zoom;
                Controls.Add(pictureBoxPoster);
            }

            if (btnDetails == null)
            {
                btnDetails = new Button();
                btnDetails.Text = "View Details";
                btnDetails.BackColor = Color.FromArgb(220, 20, 60);
                btnDetails.ForeColor = Color.White;
                btnDetails.FlatStyle = FlatStyle.Flat;
                btnDetails.Click += BtnDetails_Click;
                Controls.Add(btnDetails);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the poster image from the specified path.
        /// </summary>
        private void LoadPoster()
        {
            try
            {
                if (!string.IsNullOrEmpty(_posterPath) && File.Exists(_posterPath))
                {
                    pictureBoxPoster.Image = Image.FromFile(_posterPath);
                }
                else
                {
                    // Set a placeholder image if no poster is available
                    pictureBoxPoster.Image = null;
                    pictureBoxPoster.BackColor = Color.FromArgb(40, 40, 40);
                }
            }
            catch (Exception ex)
            {
                // Log error and use placeholder
                System.Diagnostics.Debug.WriteLine($"Error loading poster: {ex.Message}");
                pictureBoxPoster.Image = null;
            }
        }

        /// <summary>
        /// Updates the rating display with stars.
        /// </summary>
        private void UpdateRatingDisplay()
        {
            int fullStars = (int)_rating;
            bool hasHalfStar = (_rating - fullStars) >= 0.5;

            string stars = new string('★', fullStars);
            if (hasHalfStar && fullStars < 5)
            {
                stars += "½";
            }

            labelRating.Text = $"{stars}\n{_rating:F1}/5";
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Details button click event.
        /// </summary>
        private void BtnDetails_Click(object sender, EventArgs e)
        {
            DetailsClicked?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Designer Controls

        private Label labelTitle;
        private Label labelGenre;
        private Label labelRating;
        private PictureBox pictureBoxPoster;
        private Button btnDetails;

        #endregion
    }
}
