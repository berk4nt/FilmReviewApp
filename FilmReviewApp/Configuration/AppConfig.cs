// Film Review App - Configuration Reference
// This file documents configuration options and settings

namespace FilmReviewApp.Configuration
{
    /// <summary>
    /// Application configuration constants and settings.
    /// Modify these values to customize application behavior.
    /// </summary>
    public static class AppConfig
    {
        // =================================================================
        // DATABASE SETTINGS
        // =================================================================

        /// <summary>
        /// Database file name. Located in %APPDATA%\FilmReviewApp\
        /// </summary>
        public const string DATABASE_NAME = "films.db";

        /// <summary>
        /// Database connection string format.
        /// Uses SQLite Version 3 with URI support.
        /// </summary>
        public const string CONNECTION_STRING_FORMAT = "Data Source={0};Version=3;";

        /// <summary>
        /// Maximum text length for movie titles.
        /// Increase if you need longer titles (not recommended over 255).
        /// </summary>
        public const int MAX_TITLE_LENGTH = 200;

        /// <summary>
        /// Maximum length for movie descriptions.
        /// Increase for longer descriptions.
        /// </summary>
        public const int MAX_DESCRIPTION_LENGTH = 2000;

        // =================================================================
        // UI SETTINGS
        // =================================================================

        /// <summary>
        /// Main form default width in pixels.
        /// </summary>
        public const int MAIN_FORM_WIDTH = 1400;

        /// <summary>
        /// Main form default height in pixels.
        /// </summary>
        public const int MAIN_FORM_HEIGHT = 850;

        /// <summary>
        /// Movie card width in the gallery.
        /// </summary>
        public const int MOVIE_CARD_WIDTH = 200;

        /// <summary>
        /// Movie card height in the gallery.
        /// </summary>
        public const int MOVIE_CARD_HEIGHT = 320;

        /// <summary>
        /// Movie poster picture box height.
        /// </summary>
        public const int POSTER_HEIGHT = 180;

        /// <summary>
        /// Padding around flow layout panel content.
        /// </summary>
        public const int FLOW_PANEL_PADDING = 10;

        /// <summary>
        /// Margin between movie cards.
        /// </summary>
        public const int CARD_MARGIN = 10;

        // =================================================================
        // THEME COLORS (RGB)
        // =================================================================

        /// <summary>
        /// Main background color (dark gray).
        /// RGB: (30, 30, 30)
        /// </summary>
        public const int THEME_COLOR_BACKGROUND = 0x1E1E1E;

        /// <summary>
        /// Secondary background color for panels (slightly lighter).
        /// RGB: (40, 40, 40)
        /// </summary>
        public const int THEME_COLOR_SECONDARY = 0x282828;

        /// <summary>
        /// Tertiary background color for inputs (lighter still).
        /// RGB: (50, 50, 50)
        /// </summary>
        public const int THEME_COLOR_TERTIARY = 0x323232;

        /// <summary>
        /// Primary accent color (crimson red).
        /// RGB: (220, 20, 60)
        /// </summary>
        public const int THEME_COLOR_PRIMARY_ACCENT = 0xDC143C;

        /// <summary>
        /// Secondary accent color (forest green).
        /// RGB: (34, 139, 34)
        /// </summary>
        public const int THEME_COLOR_SECONDARY_ACCENT = 0x228B22;

        /// <summary>
        /// Gold color for ratings and highlights.
        /// RGB: (255, 215, 0)
        /// </summary>
        public const int THEME_COLOR_GOLD = 0xFFD700;

        /// <summary>
        /// Text color (white).
        /// RGB: (255, 255, 255)
        /// </summary>
        public const int THEME_COLOR_TEXT = 0xFFFFFF;

        /// <summary>
        /// Dimmed text color (silver).
        /// RGB: (192, 192, 192)
        /// </summary>
        public const int THEME_COLOR_DIMMED = 0xC0C0C0;

        // =================================================================
        // FORM SETTINGS
        // =================================================================

        /// <summary>
        /// Add Movie Form default width.
        /// </summary>
        public const int ADD_MOVIE_FORM_WIDTH = 500;

        /// <summary>
        /// Add Movie Form default height.
        /// </summary>
        public const int ADD_MOVIE_FORM_HEIGHT = 650;

        /// <summary>
        /// Movie Detail Form default width.
        /// </summary>
        public const int DETAIL_FORM_WIDTH = 800;

        /// <summary>
        /// Movie Detail Form default height.
        /// </summary>
        public const int DETAIL_FORM_HEIGHT = 900;

        // =================================================================
        // VALIDATION SETTINGS
        // =================================================================

        /// <summary>
        /// Minimum rating value (stars).
        /// </summary>
        public const int MIN_RATING = 1;

        /// <summary>
        /// Maximum rating value (stars).
        /// </summary>
        public const int MAX_RATING = 5;

        /// <summary>
        /// Minimum release year.
        /// </summary>
        public const int MIN_RELEASE_YEAR = 1900;

        /// <summary>
        /// Maximum release year (current year + 10 for future releases).
        /// </summary>
        public static int MAX_RELEASE_YEAR => System.DateTime.Now.Year + 10;

        // =================================================================
        // FILE SETTINGS
        // =================================================================

        /// <summary>
        /// Supported image formats for poster files.
        /// </summary>
        public const string SUPPORTED_IMAGE_FORMATS = "*.jpg;*.jpeg;*.png;*.bmp";

        /// <summary>
        /// Image filter for OpenFileDialog.
        /// </summary>
        public const string IMAGE_FILE_FILTER = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

        // =================================================================
        // FONT SETTINGS
        // =================================================================

        /// <summary>
        /// Default font family for the application.
        /// </summary>
        public const string DEFAULT_FONT = "Segoe UI";

        /// <summary>
        /// Title font size (in points).
        /// </summary>
        public const float TITLE_FONT_SIZE = 18;

        /// <summary>
        /// Subtitle font size.
        /// </summary>
        public const float SUBTITLE_FONT_SIZE = 14;

        /// <summary>
        /// Normal font size for labels and text.
        /// </summary>
        public const float NORMAL_FONT_SIZE = 10;

        /// <summary>
        /// Small font size for auxiliary text.
        /// </summary>
        public const float SMALL_FONT_SIZE = 8;

        // =================================================================
        // PERFORMANCE SETTINGS
        // =================================================================

        /// <summary>
        /// Maximum number of movies to display before pagination.
        /// Set to 0 to disable pagination.
        /// </summary>
        public const int PAGE_SIZE = 0; // Unlimited

        /// <summary>
        /// Enable debug logging to Output window.
        /// </summary>
        public const bool ENABLE_DEBUG_LOGGING = true;

        /// <summary>
        /// Cache genre list to avoid repeated database queries.
        /// </summary>
        public const bool ENABLE_GENRE_CACHING = true;

        // =================================================================
        // FEATURE FLAGS
        // =================================================================

        /// <summary>
        /// Enable movie edit functionality.
        /// </summary>
        public const bool FEATURE_EDIT_MOVIE = true;

        /// <summary>
        /// Enable movie delete functionality.
        /// </summary>
        public const bool FEATURE_DELETE_MOVIE = true;

        /// <summary>
        /// Enable review editing.
        /// </summary>
        public const bool FEATURE_EDIT_REVIEW = false;

        /// <summary>
        /// Enable review deletion.
        /// </summary>
        public const bool FEATURE_DELETE_REVIEW = false;

        /// <summary>
        /// Enable export to CSV.
        /// </summary>
        public const bool FEATURE_EXPORT_CSV = false;

        /// <summary>
        /// Enable import from CSV.
        /// </summary>
        public const bool FEATURE_IMPORT_CSV = false;

        // =================================================================
        // USAGE EXAMPLE
        // =================================================================

        /*

        // How to use these configuration constants:

        // In your forms:
        public partial class Form1 : Form
        {
            public Form1()
            {
                InitializeComponent();

                // Apply settings
                this.Width = AppConfig.MAIN_FORM_WIDTH;
                this.Height = AppConfig.MAIN_FORM_HEIGHT;
                this.BackColor = ColorTranslator.FromOle(AppConfig.THEME_COLOR_BACKGROUND);
            }
        }

        // In DatabaseHelper:
        public DatabaseHelper()
        {
            // ...validation code...
            if (rating < AppConfig.MIN_RATING || rating > AppConfig.MAX_RATING)
            {
                throw new ArgumentException(
                    $"Rating must be between {AppConfig.MIN_RATING} and {AppConfig.MAX_RATING}");
            }
        }

        // For movie cards:
        movieCardPanel.Size = new Size(AppConfig.MOVIE_CARD_WIDTH, AppConfig.MOVIE_CARD_HEIGHT);

        // For colors:
        Color bgColor = ColorTranslator.FromOle(AppConfig.THEME_COLOR_BACKGROUND);
        Color accentColor = ColorTranslator.FromOle(AppConfig.THEME_COLOR_PRIMARY_ACCENT);

        */
    }
}
