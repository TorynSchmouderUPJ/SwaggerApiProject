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
├── Dockerfile
├── .dockerignore
└── PersonApi.csproj
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [JetBrains Rider](https://www.jetbrains.com/rider/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for containerized deployment)

## Getting Started with Rider

1. Open Rider → **File → Open** → select the `PersonApi` folder or `PersonApi.csproj`
2. Rider will auto-detect the project and restore NuGet packages
3. Go to **Run → Edit Configurations** and set:
   - **Environment variables:** `ASPNETCORE_ENVIRONMENT=Development`
   - **Launch URL:** `http://localhost:5000/swagger`
4. Hit **Shift+F10** to run
5. Navigate to `http://localhost:5000/swagger` to explore the API

## Running with Docker

Build the image:

```bash
docker build -t person-api .
```

Run the container:

```bash
docker run -d -p 5000:8080 --name person-api person-api
```

Navigate to `http://localhost:5000/swagger` to access the Swagger UI.

To persist data across container rebuilds, mount a volume:

```bash
docker run -d -p 5000:8080 -v persondata:/app/Data --name person-api person-api
```

## API Endpoints

| Method | Route              | Description          |
|--------|--------------------|----------------------|
| GET    | /api/person        | Get all persons      |
| GET    | /api/person/{id}   | Get person by id     |
| POST   | /api/person        | Create a new person  |
| PUT    | /api/person/{id}   | Update a person      |
| DELETE | /api/person/{id}   | Delete a person      |

## Data Storage

Data is persisted to `Data/persons.json` relative to the build output directory (or `/app/Data/persons.json` inside Docker). The `FileManager` singleton ensures all reads and writes are thread-safe via a lock, and the file is kept in sync with an in-memory cache for fast access.
