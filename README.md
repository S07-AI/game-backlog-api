# Game Backlog Tracker API

A RESTful API for managing your personal game collection. Track games across platforms, monitor your play status, log hours, and get stats on your backlog — built with C# and ASP.NET Core.

## Tech Stack

- **C#** / **ASP.NET Core 8** — Web API framework
- **Entity Framework Core 8** — ORM for database access
- **PostgreSQL** — Relational database
- **LINQ** — Query language for filtering and aggregating data

## Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL

### Setup

1. Clone the repository
```bash
   git clone https://github.com/S07-AI/game-backlog-api.git
   cd game-backlog-api/GameBacklog
```

2. Update the connection string in `appsettings.json`
```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=gamebacklog;Username=youruser;Password=yourpassword"
   }
```

3. Apply database migrations
```bash
   dotnet ef database update
```

4. Run the API
```bash
   dotnet run
```

The API will be available at `http://localhost:5220`

## API Endpoints

### Games

| Method | Endpoint | Description |
|---|---|---|
| GET | /api/games | Get all games (supports filtering) |
| GET | /api/games/{id} | Get a game by ID |
| GET | /api/games/search?q= | Search games by title |
| POST | /api/games | Add a new game |
| PUT | /api/games/{id} | Update a game |
| DELETE | /api/games/{id} | Delete a game |

### Filtering & Sorting

GET /api/games?status=Completed

GET /api/games?genre=RPG

GET /api/games?platform=1

GET /api/games?sort=rating

GET /api/games?status=Playing&sort=hoursPlayed

### Platforms

| Method | Endpoint | Description |
|---|---|---|
| GET | /api/platforms | Get all platforms |
| POST | /api/platforms | Add a new platform |
| GET | /api/platforms/{id}/games | Get all games on a platform |

### Stats

| Method | Endpoint | Description |
|---|---|---|
| GET | /api/stats | Get backlog summary stats |

**Example stats response:**
```json
{
  "totalGames": 6,
  "gamesByStatus": [
    { "status": "Completed", "count": 2 },
    { "status": "Playing", "count": 2 },
    { "status": "Dropped", "count": 1 },
    { "status": "Wishlist", "count": 1 }
  ],
  "averageRatingOfCompleted": 9.5,
  "totalHoursPlayed": 185,
  "mostPopularPlatformId": { "platformId": 1, "count": 3 }
}
```

## Data Validation

The API enforces the following rules on game entries:

- `Title` — required, max 100 characters
- `Status` — must be one of: `Playing`, `Completed`, `Dropped`, `Wishlist`
- `Rating` — must be between 1 and 10
- `HoursPlayed` — must be between 0 and 20000