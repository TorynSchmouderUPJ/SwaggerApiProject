# Person WebAPI

A .NET 8 MVC WebAPI with JSON file persistence using a thread-safe singleton `FileManager` and **Newtonsoft.Json** for serialization.

## Project Structure

```
PersonApi/
├── Controllers/
│   └── PersonController.cs    # Strongly typed REST controller
├── Models/
│   └── Person.cs              # Person model (Id, Name, School)
├── Services/
│   └── FileManager.cs         # Thread-safe singleton – JSON file persistence
├── Properties/
│   └── launchSettings.json
├── Program.cs                 # App startup & Newtonsoft config
├── appsettings.json
└── PersonApi.csproj
```

## Quick Start

```bash
dotnet restore
dotnet run
```

Navigate to `https://localhost:7100/swagger` to explore the API via Swagger UI.

## API Endpoints

| Method | Route              | Description          |
|--------|--------------------|----------------------|
| GET    | /api/person        | Get all persons      |
| GET    | /api/person/{id}   | Get person by id     |
| POST   | /api/person        | Create a new person  |
| PUT    | /api/person/{id}   | Update a person      |
| DELETE | /api/person/{id}   | Delete a person      |

## Sample Request Body (POST / PUT)

```json
{
  "id": "1",
  "name": "Alice Johnson",
  "school": "MIT"
}
```

> If `id` is omitted on POST, a GUID will be auto-generated.

## Data Storage

Data is persisted to `Data/persons.json` (relative to the build output directory). The `FileManager` singleton ensures all reads and writes are thread-safe via a lock, and the file is kept in sync with an in-memory cache for fast access.
