# MovieApp

Навчальний ASP.NET Core MVC (.NET 11) проєкт із сервісним шаром.

## Архітектура

- `Services/IMovieService.cs`, `Services/MovieService.cs` — уся робота з БД (EF Core + SQLite) і файлами постерів (`IWebHostEnvironment`)
- `Extensions/ServiceCollectionExtensions.cs` — `AddApplicationServices()` реєструє сервіси в DI
- `Controllers/MoviesController.cs` — «тонкий» контролер, залежить лише від `IMovieService`
- `Program.cs` — підключення через `builder.Services.AddApplicationServices()`

## Запуск

```bash
dotnet run
```

БД `movies.db` створюється автоматично при першому запуску, постери зберігаються в `wwwroot/posters`.

## Версія .NET

Проєкт націлений на `net11.0`. Потрібен .NET 11 SDK (на момент створення — RC2; GA заплановано на 10 листопада 2026).
Після виходу релізу замініть у `MovieApp.csproj` версію пакета `11.0.0-rc.*` на `11.0.*`.
