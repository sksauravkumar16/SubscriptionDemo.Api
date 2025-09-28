# SubscriptionDemo

Minimal ASP.NET Core (net8.0) project using ADO.NET and JWT authentication.

## Setup

1. Open SQL Server Management Studio and run `SQL/CreateDatabase.sql` to create the database and sample data.
2. Open the solution folder in Visual Studio 2022.
3. Restore NuGet packages (`dotnet restore` or Visual Studio will do it).
4. Update `appsettings.json` connection string if necessary.
5. Run the project (F5). The frontend (`wwwroot/index.html`) should be served at the root and the API at `/api/...`.

## Notes

- Register via the frontend to create a user. Login to receive a JWT token used for creation endpoints.
