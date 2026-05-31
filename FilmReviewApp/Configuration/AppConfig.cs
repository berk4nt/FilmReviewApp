namespace FilmReviewApp.Configuration
{
    public static class AppConfig
    {
        public const string DATABASE_NAME = "films.db";

        public const string CONNECTION_STRING_FORMAT = "Data Source={0};Version=3;";

        public const int MAX_TITLE_LENGTH = 200;

        public const int MAX_DESCRIPTION_LENGTH = 2000;

        public const int MAIN_FORM_WIDTH = 1400;

        public const int MAIN_FORM_HEIGHT = 850;

        public const int MOVIE_CARD_WIDTH = 200;

        public const int MOVIE_CARD_HEIGHT = 320;

        public const int POSTER_HEIGHT = 180;

        public const int FLOW_PANEL_PADDING = 10;

        public const int CARD_MARGIN = 10;

        public const int THEME_COLOR_BACKGROUND = 0x1E1E1E;

        public const int THEME_COLOR_SECONDARY = 0x282828;

        public const int THEME_COLOR_TERTIARY = 0x323232;

        public const int THEME_COLOR_PRIMARY_ACCENT = 0xDC143C;

        public const int THEME_COLOR_SECONDARY_ACCENT = 0x228B22;

        public const int THEME_COLOR_GOLD = 0xFFD700;

        public const int THEME_COLOR_TEXT = 0xFFFFFF;

        public const int THEME_COLOR_DIMMED = 0xC0C0C0;

        public const int ADD_MOVIE_FORM_WIDTH = 500;

        public const int ADD_MOVIE_FORM_HEIGHT = 650;

        public const int DETAIL_FORM_WIDTH = 800;

        public const int DETAIL_FORM_HEIGHT = 900;

        public const int MIN_RATING = 1;

        public const int MAX_RATING = 5;

        public const int MIN_RELEASE_YEAR = 1900;

        public static int MAX_RELEASE_YEAR => System.DateTime.Now.Year + 10;

        public const string SUPPORTED_IMAGE_FORMATS = "*.jpg;*.jpeg;*.png;*.bmp";

        public const string IMAGE_FILE_FILTER = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

        public const string DEFAULT_FONT = "Segoe UI";

        public const float TITLE_FONT_SIZE = 18;

        public const float SUBTITLE_FONT_SIZE = 14;

        public const float NORMAL_FONT_SIZE = 10;

        public const float SMALL_FONT_SIZE = 8;

        public const int PAGE_SIZE = 0;

        public const bool ENABLE_DEBUG_LOGGING = true;

        public const bool ENABLE_GENRE_CACHING = true;

        public const bool FEATURE_EDIT_MOVIE = true;

        public const bool FEATURE_DELETE_MOVIE = true;

        public const bool FEATURE_EDIT_REVIEW = false;

        public const bool FEATURE_DELETE_REVIEW = false;

        public const bool FEATURE_EXPORT_CSV = false;

        public const bool FEATURE_IMPORT_CSV = false;
    }
}
