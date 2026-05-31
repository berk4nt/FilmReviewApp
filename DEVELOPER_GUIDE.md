# Film Review App - Developer Documentation

## Table of Contents
1. [Architecture Overview](#architecture-overview)
2. [Code Structure](#code-structure)
3. [Database Design](#database-design)
4. [Class Reference](#class-reference)
5. [Development Guidelines](#development-guidelines)
6. [Best Practices](#best-practices)
7. [Troubleshooting](#troubleshooting)

## Architecture Overview

### Layered Architecture

```
┌─────────────────────────────────────┐
│   Presentation Layer (Forms)        │
│  ┌─────────────────────────────────┐│
│  │ Form1 (Main Gallery)            ││
│  │ AddMovieForm (Add/Edit)         ││
│  │ MovieDetailForm (Reviews)       ││
│  │ MovieCard (User Control)        ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
           ↓ Uses
┌─────────────────────────────────────┐
│  Business Logic Layer (Models)      │
│  ┌─────────────────────────────────┐│
│  │ Movie Model                     ││
│  │ Review Model                    ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
           ↓ Uses
┌─────────────────────────────────────┐
│  Data Access Layer (DatabaseHelper) │
│  ┌─────────────────────────────────┐│
│  │ SQLite Database Operations      ││
│  │ ADO.NET Queries                 ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
           ↓ Uses
┌─────────────────────────────────────┐
│   Data Layer (SQLite Database)      │
│  ┌─────────────────────────────────┐│
│  │ Films Table                     ││
│  │ Reviews Table                   ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

## Code Structure

### Models Namespace (Data Contracts)

#### Movie.cs
```csharp
public class Movie
{
    public int FilmID { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Director { get; set; }
    public int ReleaseYear { get; set; }
    public string Description { get; set; }
    public string PosterPath { get; set; }
}
```

**Responsibility**: Represents a movie entity with all relevant properties.

#### Review.cs
```csharp
public class Review
{
    public int ReviewID { get; set; }
    public int FilmID { get; set; }
    public string UserName { get; set; }
    public int Rating { get; set; }        // 1-5
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; }
}
```

**Responsibility**: Represents a review entity linked to a movie.

### Data Namespace (Database Operations)

#### DatabaseHelper.cs
Core class handling all database operations:

**Key Methods**:

```csharp
// Movie Management
public int InsertMovie(string title, string genre, string director, int releaseYear, string description, string posterPath)
public void UpdateMovie(int filmID, string title, string genre, string director, int releaseYear, string description, string posterPath)
public void DeleteMovie(int filmID)
public List<Movie> GetAllMovies()
public Movie GetMovieById(int filmID)
public List<Movie> SearchMovies(string searchTerm, string genre = "")
public List<string> GetAllGenres()

// Review Management
public int InsertReview(int filmID, string userName, int rating, string comment)
public List<Review> GetReviewsByFilmId(int filmID)
public double GetAverageRating(int filmID)
public int GetReviewCount(int filmID)

// Database Management
private void InitializeDatabase()
private void CreateTables()
```

**Key Features**:
- **Parameterized Queries**: All SQL uses parameters to prevent injection
- **Connection Management**: Proper using statements for resource cleanup
- **Error Handling**: Comprehensive exception handling with context
- **Null Safety**: Handles null values and DBNull checks

### Forms Namespace (User Interface)

#### Form1.cs (Main Gallery Form)
**Responsibilities**:
- Display all movies in a card gallery
- Real-time search and filtering
- Genre-based filtering
- Sorting by rating and date
- Navigate to add/detail forms

**Key Components**:
- Header Panel: Search, filter, sort controls
- Flow Layout Panel: Dynamic movie card display
- Event Handlers: Search, filter, sort functionality

#### AddMovieForm.cs (Movie Creation/Editing)
**Responsibilities**:
- Provide interface for adding new movies
- Allow editing existing movies
- Image selection with file dialog
- Image preview display
- Form validation

**Key Features**:
- Modal dialog pattern
- Edit mode detection
- Image file dialog
- Input validation

#### MovieDetailForm.cs (Movie Details & Reviews)
**Responsibilities**:
- Display movie details
- Show average rating
- Display all reviews
- Allow review submission
- Validate review input

**Key Components**:
- Movie details section
- Review submission form
- DataGridView for review display
- Real-time rating updates

### Controls Namespace (Reusable Components)

#### MovieCard.cs (User Control)
**Responsibilities**:
- Display individual movie in card format
- Show poster, title, genre, rating
- Trigger detail view on button click
- Expose movie data properties

**Properties**:
```csharp
public int MovieId { get; set; }
public string MovieTitle { get; set; }
public string MovieGenre { get; set; }
public double Rating { get; set; }
public string PosterPath { get; set; }
```

**Events**:
```csharp
public event EventHandler DetailsClicked;
```

## Database Design

### Schema Diagram

```
┌──────────────────────┐         ┌──────────────────────┐
│     Films            │         │     Reviews          │
├──────────────────────┤         ├──────────────────────┤
│ FilmID (PK)          │◄────┬───│ ReviewID (PK)        │
│ Title                │     │   │ FilmID (FK)          │
│ Genre                │     │   │ UserName             │
│ Director             │     │   │ Rating               │
│ ReleaseYear          │     │   │ Comment              │
│ Description          │     │   │ ReviewDate           │
│ PosterPath           │     └───│ CASCADE DELETE       │
└──────────────────────┘         └──────────────────────┘
```

### SQL Queries Reference

**Get Average Rating with Review Count**:
```sql
SELECT AVG(CAST(Rating AS FLOAT)), COUNT(*) FROM Reviews WHERE FilmID = @filmID;
```

**Search Movies**:
```sql
SELECT * FROM Films 
WHERE (Title LIKE @searchTerm OR Genre LIKE @searchTerm)
AND (Genre = @genre OR @genre IS NULL)
ORDER BY Title ASC;
```

**Get Reviews Ordered by Date**:
```sql
SELECT * FROM Reviews 
WHERE FilmID = @filmID 
ORDER BY ReviewDate DESC;
```

## Class Reference

### DatabaseHelper

#### Constructor
```csharp
public DatabaseHelper()
```
- Creates database in AppData folder if not exists
- Initializes tables

#### Movie Operations

**InsertMovie**
```csharp
public int InsertMovie(string title, string genre, string director, int releaseYear, string description, string posterPath)
```
- **Parameters**: Movie details
- **Returns**: New movie ID
- **Throws**: Exception if database operation fails
- **SQL**: INSERT with last_insert_rowid()

**UpdateMovie**
```csharp
public void UpdateMovie(int filmID, string title, string genre, string director, int releaseYear, string description, string posterPath)
```
- **Parameters**: Movie ID and new details
- **Throws**: Exception if update fails

**DeleteMovie**
```csharp
public void DeleteMovie(int filmID)
```
- **Parameters**: Movie ID
- **Behavior**: Deletes movie and all associated reviews (CASCADE)
- **Throws**: Exception if deletion fails

**GetAllMovies**
```csharp
public List<Movie> GetAllMovies()
```
- **Returns**: List of all movies, sorted by title
- **Throws**: Exception if query fails

**SearchMovies**
```csharp
public List<Movie> SearchMovies(string searchTerm, string genre = "")
```
- **Parameters**: Search term (title/genre), optional genre filter
- **Returns**: Filtered movie list
- **Behavior**: Case-insensitive search using LIKE operator

**GetAllGenres**
```csharp
public List<string> GetAllGenres()
```
- **Returns**: List of unique genres, sorted alphabetically
- **Throws**: Exception if query fails

#### Review Operations

**InsertReview**
```csharp
public int InsertReview(int filmID, string userName, int rating, string comment)
```
- **Parameters**: Movie ID, reviewer name, rating (1-5), comment
- **Returns**: New review ID
- **Validation**: Rating must be 1-5
- **Throws**: ArgumentException if rating invalid, Exception on DB error

**GetReviewsByFilmId**
```csharp
public List<Review> GetReviewsByFilmId(int filmID)
```
- **Parameters**: Movie ID
- **Returns**: List of reviews, ordered by date (newest first)
- **Throws**: Exception if query fails

**GetAverageRating**
```csharp
public double GetAverageRating(int filmID)
```
- **Parameters**: Movie ID
- **Returns**: Average rating (0-5), or 0 if no reviews
- **Throws**: Exception if query fails

**GetReviewCount**
```csharp
public int GetReviewCount(int filmID)
```
- **Parameters**: Movie ID
- **Returns**: Number of reviews for movie
- **Throws**: Exception if query fails

## Development Guidelines

### Code Organization

#### Namespaces
```csharp
// Presentation layer
namespace FilmReviewApp.Forms { }

// Reusable controls
namespace FilmReviewApp.Controls { }

// Data models
namespace FilmReviewApp.Models { }

// Data access
namespace FilmReviewApp.Data { }

// Utilities
namespace FilmReviewApp.Utilities { }
```

#### Regions
Organize code with regions for readability:
```csharp
#region Fields
#region Properties
#region Constructor
#region Initialization
#region Data Loading
#region Event Handlers
#region Methods
#region Designer Fields
```

### Documentation Standards

**Class Documentation**:
```csharp
/// <summary>
/// Provides database operations for film and review management.
/// Uses SQLite with ADO.NET for data persistence.
/// </summary>
public class DatabaseHelper
```

**Method Documentation**:
```csharp
/// <summary>
/// Inserts a new movie into the database.
/// </summary>
/// <param name="title">The movie title.</param>
/// <param name="genre">The movie genre.</param>
/// <returns>The ID of the inserted movie.</returns>
public int InsertMovie(string title, string genre, ...)
```

**Property Documentation**:
```csharp
/// <summary>
/// Gets or sets the unique identifier for the movie.
/// </summary>
public int FilmID { get; set; }
```

### Exception Handling Pattern

```csharp
try
{
    // Operation
    // ...
}
catch (ArgumentException ex)
{
    // Handle specific argument errors
    throw new ArgumentException($"Error context: {ex.Message}", ex);
}
catch (Exception ex)
{
    // Handle all other errors with context
    throw new Exception($"Error performing operation: {ex.Message}", ex);
}
```

## Best Practices

### 1. Database Access

✅ **DO**:
```csharp
using (SQLiteCommand command = new SQLiteCommand(query, connection))
{
    command.Parameters.AddWithValue("@filmID", filmID);
    // Execute
}
```

❌ **DON'T**:
```csharp
command.CommandText = $"SELECT * FROM Films WHERE FilmID = {filmID}"; // SQL injection!
```

### 2. Resource Management

✅ **DO**:
```csharp
using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
{
    connection.Open();
    // Use connection
} // Automatically disposed
```

❌ **DON'T**:
```csharp
SQLiteConnection connection = new SQLiteConnection(_connectionString);
connection.Open();
// Forget to close/dispose
```

### 3. Null Checking

✅ **DO**:
```csharp
if (result == null || result is DBNull)
{
    return 0.0;
}
return Convert.ToDouble(result);
```

❌ **DON'T**:
```csharp
return Convert.ToDouble(result); // Crashes if null
```

### 4. Input Validation

✅ **DO**:
```csharp
if (rating < 1 || rating > 5)
{
    throw new ArgumentException("Rating must be between 1 and 5.");
}
```

❌ **DON'T**:
```csharp
// Insert without validation, risk data integrity
```

### 5. Error Messages

✅ **DO**:
```csharp
catch (Exception ex)
{
    MessageBox.Show(
        $"Error loading movies: {ex.Message}",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error);
}
```

❌ **DON'T**:
```csharp
MessageBox.Show("Error!"); // Not helpful
```

## Troubleshooting

### Database Issues

**Problem**: "Database is locked"
- **Cause**: Multiple connections simultaneously
- **Solution**: Ensure all connections are properly disposed

**Problem**: "Table already exists"
- **Cause**: CreateTables called multiple times
- **Solution**: Uses "CREATE TABLE IF NOT EXISTS" - normal behavior

**Problem**: "Cannot find database file"
- **Cause**: AppData folder path issue
- **Solution**: Check folder permissions and path

### Form Issues

**Problem**: Controls not displaying correctly
- **Solution**: Verify control creation and parent container assignment

**Problem**: DataGridView not showing data
- **Solution**: Check column names match data source columns

**Problem**: Image not loading from PosterPath
- **Solution**: Verify file exists, handle FileNotFoundException

### Common Errors

| Error | Cause | Solution |
|-------|-------|----------|
| CS0103 | Method not found | Check method name spelling, namespace |
| CS0246 | Type not found | Add using statement for namespace |
| ArgumentException | Invalid parameter | Check parameter constraints |
| FileNotFoundException | Image file missing | Add null check, provide placeholder |
| DBNull exception | Unexpected null value | Use DBNull.Value check |

### Debug Tips

**Enable Debug Logging**:
```csharp
System.Diagnostics.Debug.WriteLine($"Database operation: {query}");
```

**Breakpoint Strategy**:
1. Set breakpoint at database operation
2. Check parameters before execution
3. Inspect DataReader values
4. Verify return values

**Test Data**:
Use SampleDataInitializer to populate test database

---

## API Examples

### Complete Workflow Example

```csharp
// Initialize database
DatabaseHelper db = new DatabaseHelper();

// Add a movie
int movieId = db.InsertMovie(
    title: "Sample Movie",
    genre: "Drama",
    director: "Director Name",
    releaseYear: 2024,
    description: "Movie description",
    posterPath: "C:\\images\\poster.jpg"
);

// Get all genres
List<string> genres = db.GetAllGenres();

// Search movies
List<Movie> results = db.SearchMovies("sample", "Drama");

// Add review
db.InsertReview(movieId, "User Name", 5, "Great movie!");

// Get average rating
double avgRating = db.GetAverageRating(movieId);

// Get all reviews
List<Review> reviews = db.GetReviewsByFilmId(movieId);
```

---

**Documentation Version**: 1.0  
**Last Updated**: 2024
