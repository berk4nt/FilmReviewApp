# Technical Specification - Film Review App

## Document Information
- **Title**: Film Review App - Technical Specification
- **Version**: 1.0
- **Date**: 2024
- **Status**: Final
- **Classification**: Technical Documentation

## 1. System Requirements

### Hardware Requirements
| Component | Minimum | Recommended |
|-----------|---------|-------------|
| Processor | Pentium 4 | Intel Core i5 or higher |
| RAM | 512 MB | 4 GB |
| Disk Space | 50 MB | 100 MB |
| Monitor | 1024x768 | 1920x1080 |

### Software Requirements
| Component | Version | Requirement |
|-----------|---------|-------------|
| Windows OS | 7 SP1+ | Windows 7, 8, 8.1, 10, 11 |
| .NET Framework | 4.7.2 | Required for application |
| SQLite | 3.x | Embedded in application |
| Visual C++ Runtime | 2015+ | For SQLite support |

### Development Requirements
| Tool | Version | Purpose |
|------|---------|---------|
| Visual Studio | 2019+ | IDE for development |
| .NET Framework | 4.7.2 SDK | Compilation target |
| NuGet | Latest | Package management |

## 2. Functional Requirements

### 2.1 Movie Management
**FR-MM-01**: Display Movie Gallery
- Requirement: Application shall display all movies in a card-based gallery layout
- Acceptance: Movies displayed with poster, title, genre, and rating visible
- Priority: High

**FR-MM-02**: Add New Movie
- Requirement: Users shall be able to add new movies with complete information
- Fields: Title (required), Genre, Director, Release Year, Description, Poster Image
- Acceptance: New movie appears in gallery after save
- Priority: High

**FR-MM-03**: Edit Existing Movie
- Requirement: Users shall be able to modify movie details
- Acceptance: Changes saved to database and reflected in gallery
- Priority: Medium

**FR-MM-04**: Delete Movie
- Requirement: Users shall be able to remove movies and associated reviews
- Acceptance: Movie removed from gallery and database
- Priority: Medium

### 2.2 Search & Filter
**FR-SF-01**: Search Movies
- Requirement: Real-time search by title and genre
- Acceptance: Results update as user types
- Search algorithm: Case-insensitive LIKE operator
- Priority: High

**FR-SF-02**: Genre Filter
- Requirement: Filter movies by selected genre
- Acceptance: ComboBox with all available genres
- Priority: High

**FR-SF-03**: Sort Movies
- Requirement: Sort by rating or date
- Options: "Highest Rating" and "Newest"
- Acceptance: Gallery updates to reflect sort order
- Priority: High

### 2.3 Review Management
**FR-RM-01**: Submit Review
- Requirement: Users shall submit ratings and comments for movies
- Fields: User Name, Rating (1-5), Comment
- Acceptance: Review appears in movie details form
- Priority: High

**FR-RM-02**: View Reviews
- Requirement: Display all reviews for a movie
- Acceptance: DataGridView shows user, rating, comment, date
- Priority: High

**FR-RM-03**: Calculate Average Rating
- Requirement: Display average rating for each movie
- Calculation: Average of all review ratings
- Display: Star icons with decimal value
- Priority: High

## 3. Non-Functional Requirements

### 3.1 Performance
**NFR-PF-01**: Response Time
- Gallery load time: < 500ms for 100 movies
- Search update time: < 100ms
- Database query time: < 50ms

**NFR-PF-02**: Scalability
- Support: Up to 10,000 movies
- Support: Up to 100,000 reviews
- Database size: Up to 500MB

### 3.2 Reliability
**NFR-RL-01**: Error Handling
- All exceptions shall be caught and logged
- User-friendly error messages for all failures
- Application shall not crash on invalid input

**NFR-RL-02**: Data Integrity
- All database operations use transactions
- Foreign key constraints enforced
- Data validation before insert/update

### 3.3 Usability
**NFR-US-01**: User Interface
- Dark theme for reduced eye strain
- Intuitive navigation between forms
- Clear button labels and instructions
- Consistent styling throughout

**NFR-US-02**: Accessibility
- Keyboard navigation support
- Tab order properly defined
- Font sizes readable (minimum 10pt)
- Colors compatible with colorblind users

### 3.4 Security
**NFR-SC-01**: SQL Injection Prevention
- All queries use parameterized statements
- No string concatenation in SQL
- Input validation on all user input

**NFR-SC-02**: Data Protection
- Database stored locally (no remote transmission)
- No sensitive data stored in plain text
- Application folder permissions verified

### 3.5 Maintainability
**NFR-MT-01**: Code Quality
- XML documentation on all public members
- Consistent naming conventions
- DRY principle followed throughout
- Code organized in logical namespaces

**NFR-MT-02**: Documentation
- README.md: Complete feature documentation
- DEVELOPER_GUIDE.md: API reference
- QUICKSTART.md: User tutorial
- Inline comments for complex logic

## 4. Database Specification

### 4.1 Connection String
```
Data Source=C:\Users\[User]\AppData\Roaming\FilmReviewApp\films.db;Version=3;
```

### 4.2 Films Table
```sql
CREATE TABLE Films (
    FilmID INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL UNIQUE,
    Genre TEXT,
    Director TEXT,
    ReleaseYear INTEGER,
    Description TEXT,
    PosterPath TEXT
);

CREATE INDEX idx_Films_Genre ON Films(Genre);
CREATE INDEX idx_Films_Title ON Films(Title);
```

### 4.3 Reviews Table
```sql
CREATE TABLE Reviews (
    ReviewID INTEGER PRIMARY KEY AUTOINCREMENT,
    FilmID INTEGER NOT NULL,
    UserName TEXT NOT NULL,
    Rating INTEGER NOT NULL CHECK(Rating >= 1 AND Rating <= 5),
    Comment TEXT,
    ReviewDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY(FilmID) REFERENCES Films(FilmID) ON DELETE CASCADE
);

CREATE INDEX idx_Reviews_FilmID ON Reviews(FilmID);
CREATE INDEX idx_Reviews_ReviewDate ON Reviews(ReviewDate);
```

### 4.4 Queries

**Get Average Rating**:
```sql
SELECT AVG(CAST(Rating AS FLOAT)) as AvgRating, COUNT(*) as ReviewCount
FROM Reviews
WHERE FilmID = @filmID;
```

**Search Movies**:
```sql
SELECT * FROM Films
WHERE (Title LIKE @search OR Genre LIKE @search)
AND (Genre = @genre OR @genre = 'All')
ORDER BY Title ASC;
```

**Get Reviews with Sorting**:
```sql
SELECT ReviewID, FilmID, UserName, Rating, Comment, ReviewDate
FROM Reviews
WHERE FilmID = @filmID
ORDER BY ReviewDate DESC;
```

## 5. API Specification

### 5.1 DatabaseHelper Public Interface

```csharp
// Constructor
DatabaseHelper()

// Movie Operations
int InsertMovie(string title, string genre, string director, int releaseYear, string description, string posterPath)
void UpdateMovie(int filmID, string title, string genre, string director, int releaseYear, string description, string posterPath)
void DeleteMovie(int filmID)
Movie GetMovieById(int filmID)
List<Movie> GetAllMovies()
List<Movie> SearchMovies(string searchTerm, string genre = "")
List<string> GetAllGenres()

// Review Operations
int InsertReview(int filmID, string userName, int rating, string comment)
List<Review> GetReviewsByFilmId(int filmID)
double GetAverageRating(int filmID)
int GetReviewCount(int filmID)
```

### 5.2 Model Properties

**Movie Model**:
- FilmID: int (read-only after insert)
- Title: string (required, max 200 chars)
- Genre: string (optional)
- Director: string (optional)
- ReleaseYear: int (1900 - current year + 10)
- Description: string (optional, max 2000 chars)
- PosterPath: string (optional, valid file path)

**Review Model**:
- ReviewID: int (read-only after insert)
- FilmID: int (required, must exist in Films table)
- UserName: string (required, max 100 chars)
- Rating: int (required, 1-5 inclusive)
- Comment: string (optional, max 2000 chars)
- ReviewDate: DateTime (auto-set to current timestamp)

## 6. User Interface Specification

### 6.1 Main Form (Form1)

**Window Properties**:
- Width: 1400 pixels
- Height: 850 pixels
- Resizable: Yes
- Start Position: Center Screen
- Background: Dark gray (#1E1E1E)

**Header Section** (Height: 100px):
- Title: "🎬 Film Review App" (Gold, 18pt bold)
- Search box: TextBox with placeholder "Search movies..."
- Genre filter: ComboBox with "All" + dynamic genres
- Sort buttons: "⭐ Highest Rating" and "🆕 Newest"
- Add button: "+ Add New Movie" (Green button)

**Gallery Section** (Remaining space):
- Panel with auto-scroll enabled
- Displays movie cards in responsive grid
- Card size: 200x320 pixels
- Card margin: 10 pixels
- Padding: 10 pixels all around

### 6.2 Movie Card

**Layout**:
- Poster image: 198x180 pixels (top)
- Title: Below poster, bold, truncated if needed
- Genre: Below title, silver text, small font
- Rating: Stars and score (e.g., "⭐⭐⭐⭐⭐ 4.5/5 (12 reviews)")
- Details button: Full width, crimson color

**Interactions**:
- Hover: Background lightens
- Click Details: Opens MovieDetailForm

### 6.3 Add Movie Form

**Window Properties**:
- Width: 500 pixels
- Height: 650 pixels
- Modal: Yes (blocks main form)
- Start Position: Center Parent

**Controls**:
- Title field: TextBox (required)
- Genre field: TextBox
- Director field: TextBox
- Release Year: NumericUpDown (1900-2034)
- Description: RichTextBox (multi-line)
- Poster selection: Button + PictureBox preview
- Save button: Triggers validation and insert
- Cancel button: Closes form without saving

### 6.4 Movie Detail Form

**Window Properties**:
- Width: 800 pixels
- Height: 900 pixels
- Modal: Yes
- Start Position: Center Parent

**Sections**:
1. **Movie Info** (Top):
   - Large poster (200x300)
   - Title, Genre, Director, Year
   - Average rating with stars
   - Description text

2. **Review Form** (Middle):
   - User Name field
   - Rating NumericUpDown (1-5)
   - Comment RichTextBox
   - Submit button

3. **Reviews List** (Bottom):
   - DataGridView with columns:
     - User (100px)
     - Rating (80px)
     - Review (350px)
     - Date (120px)
   - Sorted newest first
   - Scrollable if many reviews

## 7. Color Scheme

### Color Palette
| Element | Color | RGB | Hex | Usage |
|---------|-------|-----|-----|-------|
| Background | Dark Gray | 30,30,30 | #1E1E1E | Main window background |
| Secondary | Medium Gray | 40,40,40 | #282828 | Panels, cards |
| Tertiary | Light Gray | 50,50,50 | #323232 | Input fields |
| Primary Accent | Crimson | 220,20,60 | #DC143C | Buttons, highlights |
| Secondary Accent | Forest Green | 34,139,34 | #228B22 | Add button |
| Accent Gold | Gold | 255,215,0 | #FFD700 | Ratings, titles |
| Text | White | 255,255,255 | #FFFFFF | Regular text |
| Text Dimmed | Silver | 192,192,192 | #C0C0C0 | Secondary text |

## 8. Error Handling

### Error Categories

**Database Errors**:
- Connection failures
- Query execution errors
- Constraint violations
- Transaction failures

**File Errors**:
- Image file not found
- File access denied
- Invalid file format
- Disk space insufficient

**Validation Errors**:
- Required field empty
- Invalid rating value
- Title too long
- Invalid year range

**User Errors**:
- User input handling
- Form state management
- Dialog cancellation
- Network issues (future)

### Error Response Strategy

1. **User-Friendly Message**: Non-technical language
2. **Action Item**: Suggest resolution
3. **Logging**: Log detailed error for debugging
4. **Recovery**: Allow user to continue or exit

Example:
```
Error: "Unable to load movie image. The file may have been deleted or moved.
Please select a different image file."
```

## 9. Testing Strategy

### Unit Testing
- DatabaseHelper methods (CRUD operations)
- Model validation logic
- Search algorithms
- Sorting functions
- Rating calculations

### Integration Testing
- Database transactions
- Multi-table operations
- File I/O operations
- UI event handling

### System Testing
- End-to-end workflows
- Performance under load
- Data persistence
- Error recovery

### Acceptance Testing
- User acceptance criteria
- Feature completeness
- Performance benchmarks
- Security verification

## 10. Deployment

### Build Process
```
1. Clean solution
2. Restore NuGet packages
3. Compile Release configuration
4. Run unit tests (when implemented)
5. Create deployment package
```

### Release Package Contents
```
FilmReviewApp.exe
README.md
QUICKSTART.md
LICENSE.txt
CHANGES.txt
```

### Installation
1. Extract files to Program Files
2. Run FilmReviewApp.exe
3. Database created automatically on first run
4. No additional configuration needed

## 11. Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2024 | Initial release |
| 1.1.0 | Future | Edit/Delete features |
| 2.0.0 | Future | Web API version |

## 12. Maintenance

### Support Levels
- **Level 1**: Bug fixes and patches
- **Level 2**: Minor feature additions
- **Level 3**: Major version updates

### Update Strategy
- Security patches: Priority 1
- Bug fixes: Priority 2
- Feature requests: Priority 3

---

**Technical Specification Version**: 1.0  
**Last Updated**: 2024  
**Status**: Complete and Finalized ✅
