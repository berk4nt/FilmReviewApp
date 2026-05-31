using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FilmReviewApp.Data;
using FilmReviewApp.Forms;
using FilmReviewApp.Models;

namespace FilmReviewApp
{
    /// <summary>
    /// Main form for the Film Review application.
    /// Displays movies in a card layout with search and filter capabilities.
    /// </summary>
    public partial class Form1 : Form
    {
        #region Fields

        private DatabaseHelper _databaseHelper;
        private List<Movie> _allMovies;
        private FlowLayoutPanel _flowLayoutPanel;
        private TextBox _searchTextBox;
        private ComboBox _genreComboBox;
        private Button _btnAddMovie;
        private Button _btnSortRating;
        private Button _btnSortNewest;
        private Label _labelSearch;
        private Label _labelGenre;
        private Label _labelSortBy;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Form1 class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper();
            _allMovies = new List<Movie>();
            SetupDesign();
            LoadMovies();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Sets up the visual design of the main form using the dark theme.
        /// </summary>
        private void SetupDesign()
        {
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1400, 850);
            Text = "🎬 Film İnceleme Uygulaması - Film Derecelendirme Sistemi";
            FormBorderStyle = FormBorderStyle.Sizable;

            // Create header panel
            Panel headerPanel = new Panel();
            headerPanel.BackColor = Color.FromArgb(40, 40, 40);
            headerPanel.Height = 80;
            headerPanel.Dock = DockStyle.Top;
            Controls.Add(headerPanel);

            // Create title
            Label labelTitle = new Label();
            labelTitle.Text = "🎬 Film İnceleme Uygulaması";
            labelTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            labelTitle.ForeColor = Color.Gold;
            labelTitle.Location = new Point(12, 3);
            labelTitle.Size = new Size(400, 25);
            headerPanel.Controls.Add(labelTitle);

            // Create search label
            _labelSearch = new Label();
            _labelSearch.Text = "Filmleri Ara:";
            _labelSearch.ForeColor = Color.White;
            _labelSearch.Location = new Point(12, 34);
            _labelSearch.Size = new Size(100, 18);
            headerPanel.Controls.Add(_labelSearch);

            // Create search textbox
            _searchTextBox = new TextBox();
            _searchTextBox.BackColor = Color.FromArgb(50, 50, 50);
            _searchTextBox.ForeColor = Color.White;
            _searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            _searchTextBox.Location = new Point(12, 52);
            _searchTextBox.Size = new Size(240, 22);
            _searchTextBox.TextChanged += SearchTextBox_TextChanged;
            headerPanel.Controls.Add(_searchTextBox);

            // Create genre label
            _labelGenre = new Label();
            _labelGenre.Text = "Tür:";
            _labelGenre.ForeColor = Color.White;
            _labelGenre.Location = new Point(265, 34);
            _labelGenre.Size = new Size(50, 18);
            headerPanel.Controls.Add(_labelGenre);

            // Create genre combobox
            _genreComboBox = new ComboBox();
            _genreComboBox.BackColor = Color.FromArgb(50, 50, 50);
            _genreComboBox.ForeColor = Color.White;
            _genreComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _genreComboBox.Location = new Point(265, 52);
            _genreComboBox.Size = new Size(140, 22);
            _genreComboBox.Items.Add("Tümü");
            _genreComboBox.SelectedIndex = 0;
            _genreComboBox.SelectedIndexChanged += GenreComboBox_SelectedIndexChanged;
            headerPanel.Controls.Add(_genreComboBox);

            // Create sort label
            _labelSortBy = new Label();
            _labelSortBy.Text = "Sırala:";
            _labelSortBy.ForeColor = Color.White;
            _labelSortBy.Location = new Point(420, 34);
            _labelSortBy.Size = new Size(60, 18);
            headerPanel.Controls.Add(_labelSortBy);

            // Create sort by rating button
            _btnSortRating = new Button();
            _btnSortRating.Text = "⭐ En Yüksek Puan";
            _btnSortRating.BackColor = Color.FromArgb(220, 20, 60);
            _btnSortRating.ForeColor = Color.White;
            _btnSortRating.FlatStyle = FlatStyle.Flat;
            _btnSortRating.Location = new Point(420, 52);
            _btnSortRating.Size = new Size(135, 22);
            _btnSortRating.Click += BtnSortRating_Click;
            _btnSortRating.Cursor = Cursors.Hand;
            headerPanel.Controls.Add(_btnSortRating);

            // Create sort by newest button
            _btnSortNewest = new Button();
            _btnSortNewest.Text = "🆕 En Yeni";
            _btnSortNewest.BackColor = Color.FromArgb(220, 20, 60);
            _btnSortNewest.ForeColor = Color.White;
            _btnSortNewest.FlatStyle = FlatStyle.Flat;
            _btnSortNewest.Location = new Point(563, 52);
            _btnSortNewest.Size = new Size(105, 22);
            _btnSortNewest.Click += BtnSortNewest_Click;
            _btnSortNewest.Cursor = Cursors.Hand;
            headerPanel.Controls.Add(_btnSortNewest);

            // Create add movie button
            _btnAddMovie = new Button();
            _btnAddMovie.Text = "+ Yeni Film Ekle";
            _btnAddMovie.BackColor = Color.FromArgb(34, 139, 34);
            _btnAddMovie.ForeColor = Color.White;
            _btnAddMovie.FlatStyle = FlatStyle.Flat;
            _btnAddMovie.Location = new Point(1235, 52);
            _btnAddMovie.Size = new Size(155, 22);
            _btnAddMovie.Click += BtnAddMovie_Click;
            _btnAddMovie.Cursor = Cursors.Hand;
            _btnAddMovie.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            headerPanel.Controls.Add(_btnAddMovie);

            // Create flow layout panel for movies
            _flowLayoutPanel = new FlowLayoutPanel();
            _flowLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            _flowLayoutPanel.Dock = DockStyle.Fill;
            _flowLayoutPanel.AutoScroll = true;
            _flowLayoutPanel.Padding = new Padding(8, 98, 8, 8);
            _flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            _flowLayoutPanel.WrapContents = true;
            Controls.Add(_flowLayoutPanel);
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads all movies from the database and displays them.
        /// </summary>
        private void LoadMovies()
        {
            try
            {
                _allMovies = _databaseHelper.GetAllMovies();
                LoadGenres();
                DisplayMovies(_allMovies);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filmler yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads genres from the database into the genre combobox.
        /// </summary>
        private void LoadGenres()
        {
            try
            {
                _genreComboBox.Items.Clear();
                _genreComboBox.Items.Add("Tümü");

                List<string> genres = _databaseHelper.GetAllGenres();
                foreach (string genre in genres)
                {
                    _genreComboBox.Items.Add(genre);
                }

                _genreComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genres: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Displays a list of movies in the flow layout panel.
        /// </summary>
        private void DisplayMovies(List<Movie> movies)
        {
            _flowLayoutPanel.Controls.Clear();

            if (movies.Count == 0)
            {
                Label labelNoMovies = new Label();
                labelNoMovies.Text = "Film bulunamadı. Başlamak için 'Yeni Film Ekle' butonuna tıklayın!";
                labelNoMovies.ForeColor = Color.Silver;
                labelNoMovies.Font = new Font("Segoe UI", 14);
                labelNoMovies.AutoSize = true;
                labelNoMovies.Location = new Point(20, 20);
                _flowLayoutPanel.Controls.Add(labelNoMovies);
                return;
            }

            foreach (var movie in movies)
            {
                // Create a movie card panel
                Panel movieCardPanel = new Panel();
                movieCardPanel.BackColor = Color.FromArgb(40, 40, 40);
                movieCardPanel.Size = new Size(220, 360);
                movieCardPanel.Margin = new Padding(8, 5, 8, 8);
                movieCardPanel.BorderStyle = BorderStyle.FixedSingle;
                movieCardPanel.Cursor = Cursors.Hand;

                // Add hover effect
                movieCardPanel.MouseEnter += (s, e) => movieCardPanel.BackColor = Color.FromArgb(50, 50, 50);
                movieCardPanel.MouseLeave += (s, e) => movieCardPanel.BackColor = Color.FromArgb(40, 40, 40);

                // Poster
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(218, 210);
                pictureBox.Location = new Point(1, 1);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.BackColor = Color.FromArgb(50, 50, 50);

                if (!string.IsNullOrEmpty(movie.PosterPath) && System.IO.File.Exists(movie.PosterPath))
                {
                    try
                    {
                        pictureBox.Image = Image.FromFile(movie.PosterPath);
                    }
                    catch { }
                }

                movieCardPanel.Controls.Add(pictureBox);

                // Title
                Label labelTitle = new Label();
                labelTitle.Text = movie.Title;
                labelTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                labelTitle.ForeColor = Color.White;
                labelTitle.Location = new Point(5, 215);
                labelTitle.Size = new Size(210, 35);
                labelTitle.AutoEllipsis = true;
                labelTitle.MaximumSize = new Size(210, 35);
                movieCardPanel.Controls.Add(labelTitle);

                // Genre
                Label labelGenre = new Label();
                labelGenre.Text = movie.Genre;
                labelGenre.Font = new Font("Segoe UI", 8);
                labelGenre.ForeColor = Color.Silver;
                labelGenre.Location = new Point(5, 253);
                labelGenre.Size = new Size(210, 20);
                labelGenre.AutoEllipsis = true;
                movieCardPanel.Controls.Add(labelGenre);

                // Rating
                double rating = _databaseHelper.GetAverageRating(movie.FilmID);
                int reviewCount = _databaseHelper.GetReviewCount(movie.FilmID);

                Label labelRating = new Label();
                labelRating.Text = $"⭐ {rating:F1}/5 ({reviewCount})";
                labelRating.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                labelRating.ForeColor = Color.Gold;
                labelRating.Location = new Point(5, 275);
                labelRating.Size = new Size(210, 20);
                movieCardPanel.Controls.Add(labelRating);

                // Details Button
                Button btnDetails = new Button();
                btnDetails.Text = "Detaylar";
                btnDetails.Size = new Size(210, 35);
                btnDetails.Location = new Point(5, 300);
                btnDetails.BackColor = Color.FromArgb(220, 20, 60);
                btnDetails.ForeColor = Color.White;
                btnDetails.FlatStyle = FlatStyle.Flat;
                btnDetails.Cursor = Cursors.Hand;
                btnDetails.Click += (s, e) => OpenMovieDetails(movie.FilmID);
                movieCardPanel.Controls.Add(btnDetails);

                _flowLayoutPanel.Controls.Add(movieCardPanel);
            }
        }

        #endregion

        #region Search and Filter

        /// <summary>
        /// Handles the search textbox text changed event.
        /// </summary>
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        /// <summary>
        /// Handles the genre combobox selected index changed event.
        /// </summary>
        private void GenreComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        /// <summary>
        /// Performs search and filter based on current criteria.
        /// </summary>
        private void PerformSearch()
        {
            try
            {
                string searchTerm = _searchTextBox.Text.Trim();
                string selectedGenre = _genreComboBox.SelectedItem?.ToString() ?? "Tümü";

                List<Movie> filteredMovies = _databaseHelper.SearchMovies(searchTerm, selectedGenre);
                DisplayMovies(filteredMovies);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama yapılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Sorts movies by highest rating.
        /// </summary>
        private void BtnSortRating_Click(object sender, EventArgs e)
        {
            try
            {
                List<Movie> moviesToSort = new List<Movie>();

                // Get current filtered list
                string searchTerm = _searchTextBox.Text.Trim();
                string selectedGenre = _genreComboBox.SelectedItem?.ToString() ?? "Tümü";
                moviesToSort = _databaseHelper.SearchMovies(searchTerm, selectedGenre);

                // Sort by average rating (descending)
                moviesToSort.Sort((a, b) =>
                {
                    double ratingA = _databaseHelper.GetAverageRating(a.FilmID);
                    double ratingB = _databaseHelper.GetAverageRating(b.FilmID);
                    return ratingB.CompareTo(ratingA);
                });

                DisplayMovies(moviesToSort);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Puana göre sıralama yapılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sorts movies by newest (highest FilmID first).
        /// </summary>
        private void BtnSortNewest_Click(object sender, EventArgs e)
        {
            try
            {
                List<Movie> moviesToSort = new List<Movie>();

                // Get current filtered list
                string searchTerm = _searchTextBox.Text.Trim();
                string selectedGenre = _genreComboBox.SelectedItem?.ToString() ?? "Tümü";
                moviesToSort = _databaseHelper.SearchMovies(searchTerm, selectedGenre);

                // Sort by FilmID descending (newest first)
                moviesToSort.Sort((a, b) => b.FilmID.CompareTo(a.FilmID));

                DisplayMovies(moviesToSort);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"En yeniye göre sıralama yapılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Add Movie button click event.
        /// </summary>
        private void BtnAddMovie_Click(object sender, EventArgs e)
        {
            AddMovieForm addMovieForm = new AddMovieForm();
            if (addMovieForm.ShowDialog(this) == DialogResult.OK)
            {
                LoadMovies(); // Reload movies after adding
            }
        }

        /// <summary>
        /// Opens the movie details form for a specific movie.
        /// </summary>
        private void OpenMovieDetails(int movieId)
        {
            MovieDetailForm detailForm = new MovieDetailForm(movieId);
            detailForm.ShowDialog(this);

            // Refresh display to update ratings
            LoadMovies();
        }

        #endregion
    }
}

