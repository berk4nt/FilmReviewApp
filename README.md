# Film Review App - Windows Forms Application

## Overview

Film Review App is a comprehensive Movie Review and Rating System built with C# Windows Forms (.NET Framework 4.7.2). It combines a Netflix/IMDb-style user interface with a robust SQLite database backend using ADO.NET for data operations.

The application allows users to:
- Browse and discover movies
- Search and filter movies by genre
- Add new movies with poster images
- Rate and review movies
- View movie details and all reviews
- Sort movies by rating and newest additions

## Architecture

### Project Structure

```
FilmReviewApp/
├── Data/
│   └── DatabaseHelper.cs          # SQLite database operations
├── Models/
│   ├── Movie.cs                    # Movie entity model
│   └── Review.cs                   # Review entity model
├── Forms/
│   ├── AddMovieForm.cs             # Add/Edit movie form
│   └── MovieDetailForm.cs          # Movie details and reviews form
├── Controls/
│   └── MovieCard.cs                # Reusable movie card user control
├── Form1.cs                         # Main application form
├── Program.cs                       # Application entry point
└── FilmReviewApp.csproj            # Project file
```

### Technology Stack

- **Framework**: .NET Framework 4.7.2
- **UI Framework**: Windows Forms (WinForms)
- **Database**: SQLite 3
- **Data Access**: ADO.NET with parameterized queries
- **Language**: C# 7.3
- **Design Pattern**: MVC-inspired separation of concerns

## Database Schema

### Films Table
```sql
CREATE TABLE Films (
    FilmID INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Genre TEXT,
    Director TEXT,
    ReleaseYear INTEGER,
    Description TEXT,
    PosterPath TEXT
);
```

### Reviews Table
```sql
CREATE TABLE Reviews (
    ReviewID INTEGER PRIMARY KEY AUTOINCREMENT,
    FilmID INTEGER NOT NULL,
    UserName TEXT NOT NULL,
    Rating INTEGER NOT NULL,
    Comment TEXT,
    ReviewDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY(FilmID) REFERENCES Films(FilmID) ON DELETE CASCADE
);
```

## Features

### 1. Main Form (Form1)
- **Movie Gallery**: Displays movies in an aesthetically pleasing card layout
- **Real-time Search**: Filter movies instantly by title and genre
- **Genre Filter**: ComboBox to filter movies by selected genre
- **Sorting Options**:
  - Sort by Highest Rating (descending)
  - Sort by Newest (by insertion order)
- **Add Movie Button**: Quick access to add new movies
- **Dark Theme UI**: Modern Netflix/IMDb-style interface

**Key Controls**:
- Search TextBox: Real-time filtering
- Genre ComboBox: Genre-based filtering
- FlowLayoutPanel: Dynamic movie card display
- Sorting Buttons: Rating and date sorting

### 2. Add Movie Form
- **Form Type**: Modal dialog for adding or editing movies
- **Fields**:
  - Movie Title (required)
  - Genre
  - Director
  - Release Year (NumericUpDown 1900-current year + 10)
  - Description (multi-line text)
  - Poster Image (with file dialog selection)
- **Image Preview**: Shows selected poster before saving
- **Edit Mode**: Automatically loads existing movie data when editing
- **Validation**: Ensures required fields are filled

### 3. Movie Detail Form
- **Movie Information Display**:
  - Large poster image
  - Title, Genre, Director, Release Year
  - Full description
  - Average rating with star display
  - Review count

- **Review Submission**:
  - User Name field
  - Rating (1-5 NumericUpDown)
  - Comment (RichTextBox for multi-line text)
  - Submit button with validation

- **Reviews Display**:
  - DataGridView showing all reviews
  - Columns: User, Rating, Review, Date
  - Sorted by newest first
  - Color-coded ratings (high ratings in gold)

### 4. Database Helper Class (DatabaseHelper.cs)
Comprehensive database operations using ADO.NET:

**Movie Operations**:
- `InsertMovie()`: Add new movie to database
- `UpdateMovie()`: Modify existing movie
- `DeleteMovie()`: Remove movie and associated reviews
- `GetAllMovies()`: Retrieve all movies
- `GetMovieById()`: Get specific movie details
- `SearchMovies()`: Search by title/genre with filtering
- `GetAllGenres()`: Get unique genres

**Review Operations**:
- `InsertReview()`: Add new review
- `GetReviewsByFilmId()`: Retrieve all reviews for a movie
- `GetAverageRating()`: Calculate average rating
- `GetReviewCount()`: Count reviews for a movie

**Database Management**:
- `InitializeDatabase()`: Create database if needed
- `CreateTables()`: Create schema if tables don't exist

### 5. MovieCard UserControl
Reusable movie card component with:
- **Properties**: MovieId, Title, Genre, Rating, PosterPath
- **Display**: Poster image, title, genre, star rating
- **Interaction**: Details button triggers event
- **Styling**: Dark theme card with hover effects

## Design Principles

### 1. Clean Code
- Comprehensive XML documentation for all public members
- Logical code organization with regions
- Meaningful variable and method names
- DRY (Don't Repeat Yourself) principle

### 2. Security
- **Parameterized Queries**: All SQL queries use parameters to prevent SQL injection
- **Input Validation**: User input validated before database operations
- **Exception Handling**: Try-catch blocks with informative error messages

### 3. User Interface
- **Dark Theme**: Color scheme matching modern applications
  - Background: `#1E1E1E` (Dark Gray)
  - Accents: `#DC143C` (Crimson Red)
  - Gold: `#FFD700` (for ratings)
  - Text: `#FFFFFF` (White)

- **Responsive Design**: Adaptive layout that works with different screen sizes
- **Visual Feedback**: Hover effects, color highlighting, clear button states

### 4. Data Access Pattern
- **DatabaseHelper Singleton Pattern**: Centralized database access
- **Connection Management**: Proper disposal of connections and commands
- **Error Handling**: Detailed exception messages for debugging

## Usage Guide

### Running the Application

1. **Build and Run**:
   - Open the project in Visual Studio
   - Build the solution (Debug or Release)
   - Run the application (F5)

2. **First Launch**:
   - Application automatically creates the database
   - Database stored in: `%APPDATA%\FilmReviewApp\films.db`
   - Empty movie list is shown

### Adding a Movie

1. Click "+ Add New Movie" button
2. Fill in movie details:
   - Title (required)
   - Genre
   - Director
   - Release Year
   - Description
3. Click "Select Poster" to choose an image (JPG, PNG, BMP)
4. Click "Save Movie"
5. New movie appears in the gallery

### Searching and Filtering

1. **Search**: Type in the search box to filter by title or genre
2. **Genre Filter**: Select a genre from the dropdown (All shows all movies)
3. **Sorting**:
   - Click "⭐ Highest Rating" to sort by average rating
   - Click "🆕 Newest" to sort by most recently added

### Viewing Movie Details

1. Click "View Details" on any movie card
2. View movie information and all reviews
3. Submit a new review:
   - Enter your name
   - Select rating (1-5)
   - Write your comment
   - Click "Submit Review"
4. New review appears in the DataGridView

### Editing/Deleting Movies

Future enhancement: Edit and delete functionality can be added to:
- Right-click context menu on movie cards
- Edit button in movie details form
- Delete with confirmation dialog

## Code Examples

### Searching Movies

```csharp
List<Movie> filteredMovies = _databaseHelper.SearchMovies("Avatar", "Sci-Fi");
```

### Adding a Review

```csharp
int reviewId = _databaseHelper.InsertReview(
    filmID: 1,
    userName: "John Doe",
    rating: 5,
    comment: "Amazing movie!"
);
```

### Getting Average Rating

```csharp
double avgRating = _databaseHelper.GetAverageRating(filmID: 1);
```

### Creating a New Movie

```csharp
int movieId = _databaseHelper.InsertMovie(
    title: "Inception",
    genre: "Sci-Fi",
    director: "Christopher Nolan",
    releaseYear: 2010,
    description: "A skilled thief...",
    posterPath: "C:\\posters\\inception.jpg"
);
```

## Error Handling

The application includes comprehensive error handling:

1. **Try-Catch Blocks**: All database operations and file operations
2. **User Feedback**: MessageBox dialogs inform users of success/errors
3. **Logging**: System.Diagnostics.Debug for development logging
4. **Validation**: Input validation before database operations

## Performance Considerations

1. **Parameterized Queries**: Prevents SQL injection and improves performance
2. **Lazy Loading**: Reviews loaded on demand when viewing movie details
3. **Caching**: Movie list cached to reduce database queries during filtering
4. **Efficient Sorting**: Sorting done in-memory after retrieving filtered results

## Future Enhancements

1. **Edit/Delete Movie**: Add functionality to modify or remove movies
2. **User Authentication**: Implement user login system
3. **Favorites System**: Allow users to save favorite movies
4. **Advanced Ratings**: Display distribution of ratings
5. **Movie Recommendations**: Suggest movies based on ratings
6. **Export/Import**: Backup and restore movie data
7. **Review Moderation**: Admin panel for managing reviews
8. **Multi-language Support**: Localization for different languages
9. **Movie Suggestions**: Integration with external APIs (OMDB, TMDB)
10. **Responsive Web Version**: Migrate to ASP.NET for web access

## Dependencies

- **System.Data.SQLite**: NuGet package for SQLite database support
- **.NET Framework 4.7.2**: Base framework requirement
- **Windows Forms**: Built-in .NET Framework feature

## Database Location

The SQLite database is stored at:
```
%APPDATA%\FilmReviewApp\films.db
```

On Windows, this typically expands to:
```
C:\Users\[Username]\AppData\Roaming\FilmReviewApp\films.db
```

## Code Quality Standards

### Comments and Documentation
- XML documentation on all public classes and methods
- Inline comments for complex logic
- Region markers for logical code organization

### Naming Conventions
- PascalCase for classes, methods, properties
- camelCase for local variables and parameters
- `_underscore` prefix for private fields
- Descriptive names that convey intent

### Exception Handling
- Specific exception types caught when possible
- Meaningful error messages with context
- User-friendly error dialogs

### Input Validation
- Required fields checked before processing
- String length validation
- Numeric range validation
- File path existence validation

## Testing Recommendations

1. **Unit Tests**: Test DatabaseHelper methods independently
2. **Integration Tests**: Test database operations with actual SQLite
3. **UI Tests**: Manual testing of form interactions
4. **Edge Cases**: Test with empty databases, invalid inputs, corrupted images

## License

This application is provided as-is for educational and commercial use.

## Support

For issues, questions, or contributions, refer to the code documentation and comments throughout the application.

---

**Application Version**: 1.0.0  
**Last Updated**: 2024  
**Target Framework**: .NET Framework 4.7.2
