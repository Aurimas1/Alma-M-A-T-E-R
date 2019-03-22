--Migrate db (In cmd API folder)
dotnet ef migrations add MigrationName
--example dotnet ef migrations add InitialCreate

--Update db
dotnet ef database update