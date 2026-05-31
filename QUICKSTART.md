# Film Review App - Quick Start Guide

## Installation & Setup

### Prerequisites
- Windows 7 or higher
- .NET Framework 4.7.2 or higher
- Visual Studio 2019 or higher (for development)

### Installation Steps

1. **Clone or Download Project**
   ```
   git clone <repository-url>
   cd FilmReviewApp
   ```

2. **Open in Visual Studio**
   - Open `FilmReviewApp.sln`
   - Wait for NuGet package restore to complete

3. **Build Project**
   - Press `Ctrl+Shift+B` or go to Build > Build Solution
   - Should show "Build succeeded"

4. **Run Application**
   - Press `F5` or click the Run button
   - Application window opens automatically

## First Launch Checklist

✅ Application starts successfully  
✅ Dark theme interface displays  
✅ Empty movie list shown  
✅ Database created in AppData folder  
✅ All buttons and controls are clickable  

## Quick Tutorial

### Step 1: Add Your First Movie

1. Click **+ Add New Movie** button (green, top right)
2. Fill in the form:
   - **Movie Title**: Enter a movie name (required)
   - **Genre**: e.g., "Action", "Drama", "Sci-Fi"
   - **Director**: Movie director name
   - **Release Year**: Use the spinner to select year
   - **Description**: Write a brief description
   - **Poster Image**: Click "Select Poster" to choose an image

3. Click **Save Movie**
4. Success message appears
5. Movie now visible in the gallery

### Step 2: Browse Movies

1. Movie appears as a card in the main gallery
2. Card shows:
   - Movie poster image
   - Movie title
   - Genre
   - Rating (0/5 initially - no reviews yet)
   - View Details button

### Step 3: Add a Review

1. Click **View Details** on any movie card
2. Movie detail form opens showing:
   - Large poster
   - Full movie information
   - Average rating and review count
   - Review submission form
   - Existing reviews

3. Fill review form:
   - **Your Name**: Enter your name
   - **Rating**: Select 1-5 stars
   - **Your Review**: Write a comment

4. Click **Submit Review**
5. Review appears in the list below
6. Average rating updates

### Step 4: Search & Filter

**Search Movies**:
- Type in search box at top
- Instant filtering by title or genre

**Filter by Genre**:
- Select genre from dropdown
- Shows only movies of that genre

**Sort Movies**:
- Click **⭐ Highest Rating**: Shows best rated movies first
- Click **🆕 Newest**: Shows recently added movies first

## Understanding the Interface

### Main Window (Form1)

```
┌─────────────────────────────────────────────┐
│ 🎬 Film Review App                          │
├─────────────────────────────────────────────┤
│ Search: [____] Genre: [___] ⭐ 🆕 [+Add]   │
├─────────────────────────────────────────────┤
│ ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐         │
│ │Movie │ │Movie │ │Movie │ │Movie │         │
│ │Card  │ │Card  │ │Card  │ │Card  │  ...    │
│ └──────┘ └──────┘ └──────┘ └──────┘         │
│                                              │
└─────────────────────────────────────────────┘
```

### Movie Card Layout

```
┌──────────────┐
│   Poster     │  ← Movie image
│   Image      │
├──────────────┤
│ Movie Title  │  ← Bold title
├──────────────┤
│ Genre Name   │  ← Silver text
├──────────────┤
│ ⭐ 5.0/5 (3) │  ← Rating & count
├──────────────┤
│View Details ▶│  ← Red button
└──────────────┘
```

### Movie Detail Form

```
┌─────────────────────────────────┐
│       Movie Poster | Title      │
│                    │ Genre      │
│                    │ Director   │
│                    │ ⭐ Rating  │
├─────────────────────────────────┤
│ Description Text                │
├─────────────────────────────────┤
│ Name: [_______]                 │
│ Rating: [5] Comment: [_____]    │
│         [Submit Review]         │
├─────────────────────────────────┤
│ User    Rating  Review    Date  │
├─────────────────────────────────┤
│ John    5/5     Great!    [Date]│
│ Jane    4/5     Good!     [Date]│
└─────────────────────────────────┘
```

## Tips & Tricks

### 1. Using Search Effectively
- Search works on both title AND genre
- Type partial text to find movies
- Searches are case-insensitive

### 2. Poster Images
- Supports JPG, PNG, BMP formats
- Recommended size: 300x400 pixels
- Shows placeholder if image not found

### 3. Reviews
- Ratings shown with ⭐ stars
- High ratings (4-5) highlighted in gold
- Reviews sorted by newest first

### 4. Sorting Combined
- Sorting applies to current search results
- Search + Sort + Genre filter together work seamlessly

### 5. Ratings Display
- ⭐ = 1 star, ⭐⭐ = 2 stars, etc.
- ⭐½ = Half star for 0.5 rating
- Shows decimal (e.g., 4.5/5)

## Sample Data

To test the application with sample movies, uncomment in Program.cs:

```csharp
static void Main()
{
    // Optional: Initialize sample data on first run
    // SampleDataInitializer.InitializeSampleData();

    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new Form1());
}
```

This adds 5 classic movies with 12 sample reviews.

## Database Location

Database stored at:
```
%APPDATA%\FilmReviewApp\films.db
```

On Windows 10/11:
```
C:\Users\[YourUsername]\AppData\Roaming\FilmReviewApp\films.db
```

**To backup**: Copy `films.db` to another location

**To reset**: Delete `films.db` (will be recreated empty on next run)

## Common Tasks

### Add Multiple Movies Quickly

1. Click "+ Add New Movie"
2. Enter details
3. Save
4. Repeat steps 1-3

### Add Batch Reviews

1. Open movie details
2. Submit review with your name
3. To add another review: refresh and add again

### Export Data

Currently manual:
1. Database location: `%APPDATA%\FilmReviewApp\films.db`
2. Copy `films.db` for backup
3. Can open with SQLite tools

### Change Theme

Edit Form1.cs:
```csharp
BackColor = Color.FromArgb(30, 30, 30);  // Background color
ForeColor = Color.White;                  // Text color
```

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| F5 | Run application (Visual Studio) |
| Ctrl+Shift+B | Build solution |
| Tab | Move between controls |
| Enter | Activate button/submit |
| Escape | Close dialog |

## Troubleshooting Quick Fixes

### Application won't start
- Ensure .NET Framework 4.7.2 is installed
- Try rebuilding solution
- Check Visual Studio error messages

### Database errors
- Delete `films.db` file
- Restart application
- Database will be recreated

### Images not showing
- Verify file path is correct
- Check image format (JPG/PNG/BMP)
- Try different image file

### Slow performance
- Check available disk space
- Close other applications
- Reduce number of reviews on screen

## Getting Help

1. **Check Documentation**: See README.md and DEVELOPER_GUIDE.md
2. **Debug Mode**: Run in Visual Studio for error messages
3. **Event Viewer**: Windows Event Viewer may have error details
4. **Rebuild**: Try Clean > Build solution
5. **Reset**: Delete database and restart

## Performance Notes

- First load may be slower (database creation)
- Subsequent launches are instant
- Search filtering is real-time
- Sorting processes in-memory for speed

## Security Notes

- Database stored locally (offline only)
- No network communication
- Passwords not required (single-user desktop app)
- Data stored in plain SQLite database

## File Structure

```
FilmReviewApp/
├── bin/
│   └── Debug/           ← Compiled executable
├── obj/                 ← Build artifacts
├── Data/
│   └── DatabaseHelper.cs
├── Models/
│   ├── Movie.cs
│   └── Review.cs
├── Forms/
│   ├── AddMovieForm.cs
│   └── MovieDetailForm.cs
├── Controls/
│   └── MovieCard.cs
├── Utilities/
│   └── SampleDataInitializer.cs
├── Form1.cs
├── Program.cs
└── [Configuration files]
```

## Next Steps

1. **Add Movies**: Start building your movie database
2. **Explore Features**: Try search, filter, and sorting
3. **Submit Reviews**: Rate and review movies
4. **Share**: Export database to share with others
5. **Extend**: Use DEVELOPER_GUIDE.md to add custom features

---

**Version**: 1.0  
**Last Updated**: 2024  
**Support**: See README.md and DEVELOPER_GUIDE.md for detailed documentation
