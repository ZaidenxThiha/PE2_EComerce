# PE2 E-Commerce Assignment

This repository delivers all artifacts requested in **QT2 Assignment 2**:

- `database/PE2ECommerce.sql` – complete schema + 15–30 seed rows per table ready for the named pipe SQL Server instance `np:\\.\pipe\LOCALDB#20734081\tsql\query`.
- `src/PE2.ECommerce.Domain` & `src/PE2.ECommerce.Services` – shared 3-tier core with EF Core mappings, repositories, authentication, order logic, and reporting queries.
- **Exercise 1** – `src/Exercise1.WinForms`: Windows Forms app (3-tier) with login, CRUD UIs for items/agents, order entry (one-to-many), and reporting dashboards.
- **Exercise 2** – `src/Exercise2.MvcClassic` implements the ASP.NET MVC workflow (DB-inspired via shared EF models) and `Exercise2.MvcFramework/README.md` explains how to scaffold the legacy .NET Framework variant if needed.
- **Exercise 3** – `src/Exercise3.RazorApp`: ASP.NET Core Razor Pages site with session-based login, CRUD pages, order creation, and analytical pages.
- **Exercise 4** – `src/Exercise4.MvcCore`: ASP.NET Core MVC (Code First) portal with cookie authentication, controllers, printable order reports, and filterable dashboards.
- **Exercise 5** – `tests/PE2.ECommerce.Tests`: xUnit suite that validates the authentication workflow and reporting queries on the shared service layer.

## Prerequisites

- .NET SDK 8.0 or later.
- SQL Server LocalDB accessible via the provided named pipe (or change the connection string in each project/app config).
- Windows 11 for running the WinForms client (the Razor/MVC apps are cross-platform).

## Getting Started

1. **Create the database**
   ```bash
   sqlcmd -S "np:\\.\pipe\LOCALDB#20734081\tsql\query" -i database/PE2ECommerce.sql
   ```
2. **Restore dependencies**
   ```bash
   dotnet restore
   ```
3. **Run unit tests**
   ```bash
   dotnet test PE2ECommerce.sln
   ```
4. **Launch Exercise 3 (Razor Pages)**
   ```bash
   dotnet run --project src/Exercise3.RazorApp/Exercise3.RazorApp.csproj
   ```
5. **Launch Exercise 4 (MVC Core)**
   ```bash
   dotnet run --project src/Exercise4.MvcCore/Exercise4.MvcCore.csproj
   ```
6. **Run Exercise 1 (WinForms)**
   Open `src/Exercise1.WinForms/Exercise1.WinForms.csproj` on a Windows machine and press *F5*. The login credentials seeded in SQL (e.g., `admin` / `admin`) work across every project.

## Notes

- Connection strings in every project already point to the named pipe server. Change only the database name if you restore under a different name.
- Exercise 2 requires Visual Studio on Windows; the included guide explains how to scaffold it quickly by reusing the shared EF Core schema and controllers implemented for Exercise 4.
- The shared services expose clean interfaces, making it easy to swap UI technologies or add APIs without re-writing data access logic.
