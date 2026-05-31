using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FilmReviewApp.Data;
using FilmReviewApp.Models;

namespace FilmReviewApp.Forms
{
    /// <summary>
    /// Form for displaying movie details and managing reviews.
    /// Shows movie information and allows users to submit and view reviews.
    /// </summary>
    public partial class MovieDetailForm : Form
    {
        #region Fields

        private int _movieId;
        private DatabaseHelper _databaseHelper;
        private Movie _currentMovie;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MovieDetailForm for a specific movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie to display.</param>
        public MovieDetailForm(int movieId)
        {
            InitializeComponent();
            _movieId = movieId;
            _databaseHelper = new DatabaseHelper();
            SetupDesign();
            LoadMovieData();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Sets up the visual design of the form using the dark theme.
        /// </summary>
        private void SetupDesign()
        {
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(800, 900);

            // Enable AutoScroll for scrollbar functionality
            AutoScroll = true;
            AutoScrollMargin = new Size(0, 10);

            // Configure labels for movie details
            ConfigureLabel(labelTitle);
            ConfigureLabel(labelGenre);
            ConfigureLabel(labelDirector);
            ConfigureLabel(labelReleaseYear);
            ConfigureLabel(labelDescription);
            ConfigureLabel(labelAverageRating);

            // Configure review section labels
            ConfigureLabel(labelUserName);
            ConfigureLabel(labelRating);
            ConfigureLabel(labelComment);
            ConfigureLabel(labelReviews);

            // Configure text boxes
            ConfigureTextBox(textBoxUserName);
            ConfigureTextBox(richTextBoxComment);

            // Configure numeric up down
            numericUpDownRating.BackColor = Color.FromArgb(40, 40, 40);
            numericUpDownRating.ForeColor = Color.White;
            numericUpDownRating.Minimum = 1;
            numericUpDownRating.Maximum = 5;
            numericUpDownRating.Value = 5;

            // Configure buttons
            ConfigureButton(btnSubmitReview);
            ConfigureButton(btnClose);

            // Configure DataGridView
            ConfigureDataGridView();

            // Configure picture box
            pictureBoxPoster.BackColor = Color.FromArgb(40, 40, 40);
            pictureBoxPoster.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxPoster.SizeMode = PictureBoxSizeMode.Zoom;
        }

        /// <summary>
        /// Configures a label with the dark theme.
        /// </summary>
        private void ConfigureLabel(Label label)
        {
            label.ForeColor = Color.White;
            label.BackColor = Color.FromArgb(30, 30, 30);
        }

        /// <summary>
        /// Configures a text box with the dark theme.
        /// </summary>
        private void ConfigureTextBox(Control textBox)
        {
            textBox.BackColor = Color.FromArgb(40, 40, 40);
            textBox.ForeColor = Color.White;
            if (textBox is TextBox tb)
                tb.BorderStyle = BorderStyle.FixedSingle;
            if (textBox is RichTextBox rtb)
                rtb.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Configures a button with the dark theme.
        /// </summary>
        private void ConfigureButton(Button button)
        {
            button.BackColor = Color.FromArgb(220, 20, 60);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.Height = 35;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Configures the DataGridView for displaying reviews.
        /// </summary>
        private void ConfigureDataGridView()
        {
            dataGridViewReviews.BackgroundColor = Color.FromArgb(40, 40, 40);
            dataGridViewReviews.ForeColor = Color.White;
            dataGridViewReviews.GridColor = Color.FromArgb(60, 60, 60);
            dataGridViewReviews.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dataGridViewReviews.DefaultCellStyle.ForeColor = Color.White;
            dataGridViewReviews.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 20, 60);
            dataGridViewReviews.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dataGridViewReviews.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gold;
            dataGridViewReviews.ReadOnly = true;
            dataGridViewReviews.AllowUserToAddRows = false;
            dataGridViewReviews.AllowUserToDeleteRows = false;
            dataGridViewReviews.AllowUserToResizeRows = false;
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads the movie data from the database and displays it.
        /// </summary>
        private void LoadMovieData()
        {
            try
            {
                _currentMovie = _databaseHelper.GetMovieById(_movieId);
                if (_currentMovie == null)
                {
                    MessageBox.Show("Film bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                // Display movie details
                Text = _currentMovie.Title;
                labelTitle.Text = _currentMovie.Title;
                labelGenre.Text = $"Tür: {_currentMovie.Genre}";
                labelDirector.Text = $"Yönetmen: {_currentMovie.Director}";
                labelReleaseYear.Text = $"Yayın Yılı: {_currentMovie.ReleaseYear}";
                labelDescription.Text = $"Açıklama:\n\n{_currentMovie.Description}";

                // Load poster
                if (!string.IsNullOrEmpty(_currentMovie.PosterPath) && File.Exists(_currentMovie.PosterPath))
                {
                    pictureBoxPoster.Image = Image.FromFile(_currentMovie.PosterPath);
                }

                // Display average rating
                double averageRating = _databaseHelper.GetAverageRating(_movieId);
                int reviewCount = _databaseHelper.GetReviewCount(_movieId);
                DisplayRating(averageRating, reviewCount);

                // Load reviews
                LoadReviews();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Film verileri yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Displays the average rating with stars.
        /// </summary>
        private void DisplayRating(double averageRating, int reviewCount)
        {
            int fullStars = (int)averageRating;
            bool hasHalfStar = (averageRating - fullStars) >= 0.5;

            string stars = new string('★', fullStars);
            if (hasHalfStar && fullStars < 5)
            {
                stars += "½";
            }

            labelAverageRating.Text = $"Ortalama Puan: {stars} {averageRating:F1}/5 ({reviewCount} inceleme)";
            labelAverageRating.ForeColor = Color.Gold;
        }

        /// <summary>
        /// Loads and displays all reviews for the movie.
        /// </summary>
        private void LoadReviews()
        {
            try
            {
                List<Review> reviews = _databaseHelper.GetReviewsByFilmId(_movieId);

                // Clear existing rows
                dataGridViewReviews.Rows.Clear();

                // Define columns
                if (dataGridViewReviews.Columns.Count == 0)
                {
                    dataGridViewReviews.Columns.Add("UserName", "Kullanıcı");
                    dataGridViewReviews.Columns.Add("Rating", "Puan");
                    dataGridViewReviews.Columns.Add("Comment", "İnceleme");
                    dataGridViewReviews.Columns.Add("ReviewDate", "Tarih");

                    dataGridViewReviews.Columns[0].Width = 100;
                    dataGridViewReviews.Columns[1].Width = 80;
                    dataGridViewReviews.Columns[2].Width = 350;
                    dataGridViewReviews.Columns[3].Width = 120;
                }

                // Add review data
                foreach (var review in reviews)
                {
                    int rowIndex = dataGridViewReviews.Rows.Add();
                    DataGridViewRow row = dataGridViewReviews.Rows[rowIndex];

                    row.Cells[0].Value = review.UserName;
                    row.Cells[1].Value = $"{review.Rating}/5";
                    row.Cells[2].Value = review.Comment;
                    row.Cells[3].Value = review.ReviewDate.ToString("yyyy-MM-dd HH:mm");

                    // Highlight high ratings in gold
                    if (review.Rating >= 4)
                    {
                        row.Cells[1].Style.ForeColor = Color.Gold;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İncelemeler yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form Controls Creation

        /// <summary>
        /// Creates all form controls at runtime.
        /// </summary>
        private void InitializeComponent()
        {
            labelTitle = new Label();
            labelGenre = new Label();
            labelDirector = new Label();
            labelReleaseYear = new Label();
            labelDescription = new Label();
            labelAverageRating = new Label();
            pictureBoxPoster = new PictureBox();

            labelUserName = new Label();
            textBoxUserName = new TextBox();
            labelRating = new Label();
            numericUpDownRating = new NumericUpDown();
            labelComment = new Label();
            richTextBoxComment = new RichTextBox();
            btnSubmitReview = new Button();

            labelReviews = new Label();
            dataGridViewReviews = new DataGridView();
            btnDelete = new Button();
            btnClose = new Button();

            ((System.ComponentModel.ISupportInitialize)(pictureBoxPoster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(numericUpDownRating)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridViewReviews)).BeginInit();
            SuspendLayout();

            // Picture Box
            pictureBoxPoster.Location = new Point(12, 12);
            pictureBoxPoster.Size = new Size(200, 300);
            pictureBoxPoster.TabIndex = 0;

            // Movie Details
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            labelTitle.Location = new Point(220, 12);
            labelTitle.Size = new Size(100, 30);
            labelTitle.TabIndex = 1;

            labelGenre.AutoSize = true;
            labelGenre.Location = new Point(220, 50);
            labelGenre.Size = new Size(100, 20);
            labelGenre.TabIndex = 2;

            labelDirector.AutoSize = true;
            labelDirector.Location = new Point(220, 75);
            labelDirector.Size = new Size(100, 20);
            labelDirector.TabIndex = 3;

            labelReleaseYear.AutoSize = true;
            labelReleaseYear.Location = new Point(220, 100);
            labelReleaseYear.Size = new Size(100, 20);
            labelReleaseYear.TabIndex = 4;

            labelAverageRating.AutoSize = true;
            labelAverageRating.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            labelAverageRating.Location = new Point(220, 125);
            labelAverageRating.Size = new Size(300, 25);
            labelAverageRating.TabIndex = 5;

            labelDescription.AutoSize = false;
            labelDescription.Location = new Point(12, 320);
            labelDescription.Size = new Size(760, 80);
            labelDescription.TabIndex = 6;
            labelDescription.TextAlign = ContentAlignment.TopLeft;

            // Review Section
            labelUserName.AutoSize = true;
            labelUserName.Location = new Point(12, 410);
            labelUserName.Text = "Adınız:";
            labelUserName.TabIndex = 7;

            textBoxUserName.Location = new Point(12, 428);
            textBoxUserName.Size = new Size(300, 23);
            textBoxUserName.TabIndex = 8;

            labelRating.AutoSize = true;
            labelRating.Location = new Point(320, 410);
            labelRating.Text = "Puan (1-5):";
            labelRating.TabIndex = 9;

            numericUpDownRating.Location = new Point(320, 428);
            numericUpDownRating.Size = new Size(100, 23);
            numericUpDownRating.TabIndex = 10;

            labelComment.AutoSize = true;
            labelComment.Location = new Point(12, 460);
            labelComment.Text = "Yorumunuz:";
            labelComment.TabIndex = 11;

            richTextBoxComment.Location = new Point(12, 478);
            richTextBoxComment.Size = new Size(760, 80);
            richTextBoxComment.TabIndex = 12;

            btnSubmitReview.Location = new Point(670, 428);
            btnSubmitReview.Size = new Size(102, 35);
            btnSubmitReview.TabIndex = 13;
            btnSubmitReview.Text = "Gönder";
            btnSubmitReview.Click += BtnSubmitReview_Click;

            // Reviews Grid
            labelReviews.AutoSize = true;
            labelReviews.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            labelReviews.Location = new Point(12, 565);
            labelReviews.Text = "İncelemeler:";
            labelReviews.TabIndex = 14;

            dataGridViewReviews.AllowUserToAddRows = false;
            dataGridViewReviews.AllowUserToDeleteRows = false;
            dataGridViewReviews.AllowUserToResizeRows = false;
            dataGridViewReviews.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewReviews.Location = new Point(12, 590);
            dataGridViewReviews.Size = new Size(760, 250);
            dataGridViewReviews.TabIndex = 15;

            // Delete Button
            btnDelete = new Button();
            btnDelete.Location = new Point(12, 850);
            btnDelete.Size = new Size(102, 35);
            btnDelete.TabIndex = 17;
            btnDelete.Text = "🗑️ Filmi Sil";
            btnDelete.BackColor = Color.FromArgb(220, 20, 60);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Click += BtnDelete_Click;

            // Close Button
            btnClose.Location = new Point(670, 850);
            btnClose.Size = new Size(102, 35);
            btnClose.TabIndex = 16;
            btnClose.Text = "Kapat";
            btnClose.Click += BtnClose_Click;

            // MovieDetailForm
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 897);
            Controls.Add(pictureBoxPoster);
            Controls.Add(labelTitle);
            Controls.Add(labelGenre);
            Controls.Add(labelDirector);
            Controls.Add(labelReleaseYear);
            Controls.Add(labelAverageRating);
            Controls.Add(labelDescription);
            Controls.Add(labelUserName);
            Controls.Add(textBoxUserName);
            Controls.Add(labelRating);
            Controls.Add(numericUpDownRating);
            Controls.Add(labelComment);
            Controls.Add(richTextBoxComment);
            Controls.Add(btnSubmitReview);
            Controls.Add(labelReviews);
            Controls.Add(dataGridViewReviews);
            Controls.Add(btnDelete);
            Controls.Add(btnClose);

            ((System.ComponentModel.ISupportInitialize)(pictureBoxPoster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(numericUpDownRating)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dataGridViewReviews)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Submit Review button click event.
        /// Validates input and saves the review to the database.
        /// </summary>
        private void BtnSubmitReview_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(textBoxUserName.Text))
                {
                    MessageBox.Show("Lütfen adınızı girin.", "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(richTextBoxComment.Text))
                {
                    MessageBox.Show("Lütfen bir yorum girin.", "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string userName = textBoxUserName.Text.Trim();
                int rating = (int)numericUpDownRating.Value;
                string comment = richTextBoxComment.Text.Trim();

                // Insert review
                _databaseHelper.InsertReview(_movieId, userName, rating, comment);

                MessageBox.Show("Yorum başarıyla gönderildi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear form
                textBoxUserName.Clear();
                richTextBoxComment.Clear();
                numericUpDownRating.Value = 5;

                // Reload reviews
                LoadReviews();

                // Update average rating
                double averageRating = _databaseHelper.GetAverageRating(_movieId);
                int reviewCount = _databaseHelper.GetReviewCount(_movieId);
                DisplayRating(averageRating, reviewCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yorum gönderilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Close button click event.
        /// </summary>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Delete Movie button click event.
        /// Deletes the movie and all associated reviews after confirmation.
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Confirmation dialog
                DialogResult result = MessageBox.Show(
                    $"'{_currentMovie.Title}' filmini silmek istediğinizden emin misiniz?\n\nBu işlem geri alınamaz ve tüm yorumlar silinecektir.",
                    "Silmeyi Onayla",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Delete the movie
                _databaseHelper.DeleteMovie(_movieId);

                MessageBox.Show("Film başarıyla silindi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the form and notify parent
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Film silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Designer Fields

        private Label labelTitle;
        private Label labelGenre;
        private Label labelDirector;
        private Label labelReleaseYear;
        private Label labelDescription;
        private Label labelAverageRating;
        private PictureBox pictureBoxPoster;

        private Label labelUserName;
        private TextBox textBoxUserName;
        private Label labelRating;
        private NumericUpDown numericUpDownRating;
        private Label labelComment;
        private RichTextBox richTextBoxComment;
        private Button btnSubmitReview;

        private Label labelReviews;
        private DataGridView dataGridViewReviews;
        private Button btnDelete;
        private Button btnClose;

        #endregion
    }
}
