using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FilmReviewApp.Data;

namespace FilmReviewApp.Forms
{
    /// <summary>
    /// Form for adding or editing movies.
    /// Allows users to input movie details and select a poster image.
    /// </summary>
    public partial class AddMovieForm : Form
    {
        #region Fields

        private DatabaseHelper _databaseHelper;
        private string _selectedPosterPath;
        private int _editingMovieId;
        private bool _isEditMode;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AddMovieForm for adding a new movie.
        /// </summary>
        public AddMovieForm()
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper();
            _editingMovieId = -1;
            _isEditMode = false;
            SetupDesign();
        }

        /// <summary>
        /// Initializes a new instance of the AddMovieForm for editing an existing movie.
        /// </summary>
        public AddMovieForm(int movieId)
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper();
            _editingMovieId = movieId;
            _isEditMode = true;
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
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(500, 650);

            // Enable AutoScroll for scrollbar functionality
            AutoScroll = true;
            AutoScrollMargin = new Size(0, 10);

            Text = _isEditMode ? "Filmi Düzenle" : "Yeni Film Ekle";

            // Configure all text boxes
            ConfigureTextBox(textBoxTitle);
            ConfigureTextBox(textBoxGenre);
            ConfigureTextBox(textBoxDirector);
            ConfigureTextBox(textBoxDescription);
            ConfigureNumericUpDown();

            // Configure buttons
            ConfigureButton(btnSelectPoster);
            ConfigureButton(btnSave);
            ConfigureButton(btnCancel);

            // Configure labels
            ConfigureLabel(labelTitle);
            ConfigureLabel(labelGenre);
            ConfigureLabel(labelDirector);
            ConfigureLabel(labelReleaseYear);
            ConfigureLabel(labelDescription);
            ConfigureLabel(labelPoster);

            // Configure picture box
            pictureBoxPreview.BackColor = Color.FromArgb(40, 40, 40);
            pictureBoxPreview.BorderStyle = BorderStyle.FixedSingle;

            // Set title
            labelFormTitle.Text = _isEditMode ? "Film Detaylarını Düzenle" : "Yeni Film Ekle";
            labelFormTitle.ForeColor = Color.Gold;
            labelFormTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        }

        /// <summary>
        /// Configures a text box with the dark theme.
        /// </summary>
        private void ConfigureTextBox(TextBox textBox)
        {
            textBox.BackColor = Color.FromArgb(40, 40, 40);
            textBox.ForeColor = Color.White;
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Configures the numeric up down control.
        /// </summary>
        private void ConfigureNumericUpDown()
        {
            numericUpDownYear.BackColor = Color.FromArgb(40, 40, 40);
            numericUpDownYear.ForeColor = Color.White;
            numericUpDownYear.Minimum = 1900;
            numericUpDownYear.Maximum = DateTime.Now.Year + 10;
            numericUpDownYear.Value = DateTime.Now.Year;
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
        /// Configures a label with the dark theme.
        /// </summary>
        private void ConfigureLabel(Label label)
        {
            label.ForeColor = Color.White;
            label.BackColor = Color.FromArgb(30, 30, 30);
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Loads movie data when in edit mode.
        /// </summary>
        private void LoadMovieData()
        {
            try
            {
                var movie = _databaseHelper.GetMovieById(_editingMovieId);
                if (movie != null)
                {
                    textBoxTitle.Text = movie.Title;
                    textBoxGenre.Text = movie.Genre;
                    textBoxDirector.Text = movie.Director;
                    numericUpDownYear.Value = movie.ReleaseYear;
                    textBoxDescription.Text = movie.Description;
                    _selectedPosterPath = movie.PosterPath;

                    // Load poster preview
                    if (!string.IsNullOrEmpty(_selectedPosterPath) && File.Exists(_selectedPosterPath))
                    {
                        pictureBoxPreview.Image = Image.FromFile(_selectedPosterPath);
                        labelPosterPath.Text = Path.GetFileName(_selectedPosterPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading movie data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form Controls Creation

        /// <summary>
        /// Creates all form controls at runtime.
        /// </summary>
        private void InitializeComponent()
        {
            labelFormTitle = new Label();
            labelTitle = new Label();
            textBoxTitle = new TextBox();
            labelGenre = new Label();
            textBoxGenre = new TextBox();
            labelDirector = new Label();
            textBoxDirector = new TextBox();
            labelReleaseYear = new Label();
            numericUpDownYear = new NumericUpDown();
            labelDescription = new Label();
            textBoxDescription = new TextBox();
            labelPoster = new Label();
            labelPosterPath = new Label();
            btnSelectPoster = new Button();
            pictureBoxPreview = new PictureBox();
            btnSave = new Button();
            btnCancel = new Button();

            ((System.ComponentModel.ISupportInitialize)(numericUpDownYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxPreview)).BeginInit();
            SuspendLayout();

            // labelFormTitle
            labelFormTitle.AutoSize = false;
            labelFormTitle.Location = new Point(12, 12);
            labelFormTitle.Size = new Size(460, 30);
            labelFormTitle.TabIndex = 0;

            // labelTitle
            labelTitle.AutoSize = true;
            labelTitle.Location = new Point(12, 50);
            labelTitle.Text = "Film Adı:";
            labelTitle.TabIndex = 1;

            // textBoxTitle
            textBoxTitle.Location = new Point(12, 68);
            textBoxTitle.Size = new Size(460, 23);
            textBoxTitle.TabIndex = 2;

            // labelGenre
            labelGenre.AutoSize = true;
            labelGenre.Location = new Point(12, 95);
            labelGenre.Text = "Tür:";
            labelGenre.TabIndex = 3;

            // textBoxGenre
            textBoxGenre.Location = new Point(12, 113);
            textBoxGenre.Size = new Size(460, 23);
            textBoxGenre.TabIndex = 4;

            // labelDirector
            labelDirector.AutoSize = true;
            labelDirector.Location = new Point(12, 140);
            labelDirector.Text = "Yönetmen:";
            labelDirector.TabIndex = 5;

            // textBoxDirector
            textBoxDirector.Location = new Point(12, 158);
            textBoxDirector.Size = new Size(460, 23);
            textBoxDirector.TabIndex = 6;

            // labelReleaseYear
            labelReleaseYear.AutoSize = true;
            labelReleaseYear.Location = new Point(12, 185);
            labelReleaseYear.Text = "Yayın Yılı:";
            labelReleaseYear.TabIndex = 7;

            // numericUpDownYear
            numericUpDownYear.Location = new Point(12, 203);
            numericUpDownYear.Size = new Size(200, 23);
            numericUpDownYear.TabIndex = 8;

            // labelDescription
            labelDescription.AutoSize = true;
            labelDescription.Location = new Point(12, 230);
            labelDescription.Text = "Açıklama:";
            labelDescription.TabIndex = 9;

            // textBoxDescription
            textBoxDescription.Location = new Point(12, 248);
            textBoxDescription.Multiline = true;
            textBoxDescription.ScrollBars = ScrollBars.Vertical;
            textBoxDescription.Size = new Size(460, 80);
            textBoxDescription.TabIndex = 10;

            // labelPoster
            labelPoster.AutoSize = true;
            labelPoster.Location = new Point(12, 335);
            labelPoster.Text = "Poster Görseli:";
            labelPoster.TabIndex = 11;

            // labelPosterPath
            labelPosterPath.AutoSize = true;
            labelPosterPath.Location = new Point(12, 353);
            labelPosterPath.Text = "No poster selected";
            labelPosterPath.ForeColor = Color.Silver;
            labelPosterPath.TabIndex = 12;

            // btnSelectPoster
            btnSelectPoster.Location = new Point(12, 371);
            btnSelectPoster.Size = new Size(150, 35);
            btnSelectPoster.TabIndex = 13;
            btnSelectPoster.Text = "Poster Seç";
            btnSelectPoster.Click += BtnSelectPoster_Click;

            // pictureBoxPreview
            pictureBoxPreview.Location = new Point(170, 371);
            pictureBoxPreview.Size = new Size(302, 100);
            pictureBoxPreview.TabIndex = 14;
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;

            // btnSave
            btnSave.Location = new Point(310, 485);
            btnSave.Size = new Size(162, 35);
            btnSave.TabIndex = 15;
            btnSave.Text = _isEditMode ? "Filmi Güncelleştir" : "Filmi Kaydet";
            btnSave.Click += BtnSave_Click;

            // btnCancel
            btnCancel.Location = new Point(12, 485);
            btnCancel.Size = new Size(162, 35);
            btnCancel.TabIndex = 16;
            btnCancel.Text = "İptal";
            btnCancel.Click += BtnCancel_Click;

            // AddMovieForm
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 542);
            Controls.Add(labelFormTitle);
            Controls.Add(labelTitle);
            Controls.Add(textBoxTitle);
            Controls.Add(labelGenre);
            Controls.Add(textBoxGenre);
            Controls.Add(labelDirector);
            Controls.Add(textBoxDirector);
            Controls.Add(labelReleaseYear);
            Controls.Add(numericUpDownYear);
            Controls.Add(labelDescription);
            Controls.Add(textBoxDescription);
            Controls.Add(labelPoster);
            Controls.Add(labelPosterPath);
            Controls.Add(btnSelectPoster);
            Controls.Add(pictureBoxPreview);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            ((System.ComponentModel.ISupportInitialize)(numericUpDownYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxPreview)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Select Poster button click event.
        /// Opens a file dialog to select an image file.
        /// </summary>
        private void BtnSelectPoster_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Film Posteri Seç";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _selectedPosterPath = openFileDialog.FileName;
                    pictureBoxPreview.Image = Image.FromFile(_selectedPosterPath);
                    labelPosterPath.Text = Path.GetFileName(_selectedPosterPath);
                }
            }
        }

        /// <summary>
        /// Handles the Save button click event.
        /// Validates input and saves the movie to the database.
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (!ValidateInput())
                {
                    return;
                }

                string title = textBoxTitle.Text.Trim();
                string genre = textBoxGenre.Text.Trim();
                string director = textBoxDirector.Text.Trim();
                int releaseYear = (int)numericUpDownYear.Value;
                string description = textBoxDescription.Text.Trim();
                string posterPath = _selectedPosterPath ?? string.Empty;

                if (_isEditMode)
                {
                    _databaseHelper.UpdateMovie(_editingMovieId, title, genre, director, releaseYear, description, posterPath);
                    MessageBox.Show("Film ba\u015far\u0131yla g\u00fcncelendi!", "Ba\u015far\u0131", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _databaseHelper.InsertMovie(title, genre, director, releaseYear, description, posterPath);
                    MessageBox.Show("Film ba\u015far\u0131yla eklendi!", "Ba\u015far\u0131", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Film kaydedilirken hata olu\u015ftu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Cancel button click event.
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates the user input.
        /// </summary>
        /// <returns>True if all input is valid; otherwise false.</returns>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                MessageBox.Show("Lütfen film adı girin.", "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (textBoxTitle.Text.Length > 200)
            {
                MessageBox.Show("Film başlığı 200 karakteri aşmamalıdır.", "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        #endregion

        #region Designer Fields

        private Label labelFormTitle;
        private Label labelTitle;
        private TextBox textBoxTitle;
        private Label labelGenre;
        private TextBox textBoxGenre;
        private Label labelDirector;
        private TextBox textBoxDirector;
        private Label labelReleaseYear;
        private NumericUpDown numericUpDownYear;
        private Label labelDescription;
        private TextBox textBoxDescription;
        private Label labelPoster;
        private Label labelPosterPath;
        private Button btnSelectPoster;
        private PictureBox pictureBoxPreview;
        private Button btnSave;
        private Button btnCancel;

        #endregion
    }
}
