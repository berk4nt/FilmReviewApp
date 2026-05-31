using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FilmReviewApp.Controls
{
    public partial class MovieCard : UserControl
    {
        private int _movieId;
        private string _title;
        private string _genre;
        private double _rating;
        private string _posterPath;

        public int MovieId
        {
            get { return _movieId; }
            set { _movieId = value; }
        }

        public string MovieTitle
        {
            get { return _title; }
            set
            {
                _title = value;
                labelTitle.Text = value;
            }
        }

        public string MovieGenre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                labelGenre.Text = value;
            }
        }

        public double Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                UpdateRatingDisplay();
            }
        }

        public string PosterPath
        {
            get { return _posterPath; }
            set
            {
                _posterPath = value;
                LoadPoster();
            }
        }

        public event EventHandler DetailsClicked;

        public MovieCard()
        {
            SetupDesign();
        }

        private void SetupDesign()
        {
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;

            BorderStyle = BorderStyle.FixedSingle;

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
                    pictureBoxPoster.Image = null;
                    pictureBoxPoster.BackColor = Color.FromArgb(40, 40, 40);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading poster: {ex.Message}");
                pictureBoxPoster.Image = null;
            }
        }

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

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            DetailsClicked?.Invoke(this, EventArgs.Empty);
        }

        private Label labelTitle;
        private Label labelGenre;
        private Label labelRating;
        private PictureBox pictureBoxPoster;
        private Button btnDetails;
    }
}
