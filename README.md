# ClinicApp

ClinicApp is an ASP.NET Core application (Razor Pages / MVC) for managing clinic operations — patients, appointments, therapists, equipment, treatment plans and payments. The solution uses Entity Framework Core for data access and ASP.NET Core Identity for authentication.

## Tech stack
- .NET 10
- ASP.NET Core (Razor Views / MVC)
- Entity Framework Core (Migrations included)
- ASP.NET Core Identity (Area: `Identity`)
- SQL Server (or any EF Core provider via `appsettings.json`)

## Prerequisites
- .NET 10 SDK
- A supported database server (SQL Server recommended)
- Visual Studio 2022/2026 or Visual Studio Code

## Getting started
1. Clone the repository:

   git clone <repo-url>

2. Open the solution `ClinicApp.slnx` in Visual Studio or use the CLI:

   dotnet restore

3. Configure the connection string in `appsettings.json` or `appsettings.Development.json`.

4. Apply database migrations and seed (if required):

   dotnet ef database update --project ClinicApp

   (Or use the Package Manager Console in Visual Studio targeting the `ClinicApp` project.)

5. Run the app:

   dotnet run --project ClinicApp

   Or launch from Visual Studio (IIS Express or Project) — default launch profiles are in `Properties/launchSettings.json`.

## Project structure
- `ClinicApp/Controllers` — MVC controllers for the web UI.
- `ClinicApp/Views` — Razor views for each controller and shared layout files.
- `ClinicApp/Areas/Identity` — Identity UI pages and account management.
- `ClinicApp/Models` — EF Core model classes.
- `ClinicApp/Data` — `ClinicAppContext` and Identity user/role classes.
- `ClinicApp/Repository` — Repository interfaces and EF-based implementations.
- `ClinicApp/Service` — Application services (business logic) used by controllers.
- `ClinicApp/ViewModel` — View models used to transfer data between controllers and views.
- `ClinicApp/Migrations` — EF Core migrations for schema history.
- `ClinicApp/wwwroot` — Static assets (CSS, JS, images).

## Authentication
The solution uses ASP.NET Core Identity (see `Areas/Identity`). Default Identity setup is included and integrated with `ApplicationUser` / `ApplicationRole` in `Data`.

## Database & Migrations
- Migrations are present in the `Migrations` folder. Use `dotnet ef migrations add <Name>` to create a new migration and `dotnet ef database update` to apply.
- If you change entity keys or relationships, update migrations and test against a copy of the database before applying to production.

## Common tasks
- Add a new service and repository:
  - Create an interface under `Service/interface` and implementation under `Service/class`.
  - Create repository interface under `Repository/interface` and implementation under `Repository/class`.
  - Register service and repository in DI (see `Program.cs`).

- Soft deletes: models include an `isDeleted` flag and a base `ILogsAttribuite` interface used by repository filtering. Review repository logic when modifying delete/update flows.

## Development tips
- Use `AsNoTracking()` on read-only queries to reduce change-tracking overhead.
- For single-column updates (e.g., soft-delete flag) consider EF Core `ExecuteUpdateAsync` (EF Core 7+) or attach-stub pattern to avoid an extra SELECT.
- Keep `SaveChangesAsync()` calls grouped where possible to minimize transactions and database round-trips.

## Tests and Benchmarks
There are no automated unit tests or Benchmark.NET benchmarks included in the repository by default. Add tests under a separate test project and use `dotnet test` to run them.

## Contributing
- Fork the repository and use feature branches. Submit pull requests with clear descriptions and, where appropriate, include database migration scripts for schema changes.

## License
Specify a license for the project here (e.g., MIT) or add a `LICENSE` file to the repository.

