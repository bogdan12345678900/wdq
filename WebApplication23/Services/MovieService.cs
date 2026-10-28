using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Models;

namespace MovieApp.Services;

public class MovieService : IMovieService
{
    private const string PostersFolder = "posters";
    private const long MaxPosterSize = 5 * 1024 * 1024; // 5 MB
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public MovieService(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<List<Movie>> GetAllAsync() =>
        await _context.Movies.AsNoTracking().OrderBy(m => m.Title).ToListAsync();

    public async Task<Movie?> GetByIdAsync(int id) =>
        await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

    public async Task CreateAsync(Movie movie, IFormFile? poster)
    {
        if (poster is { Length: > 0 })
            movie.PosterPath = await SavePosterAsync(poster);

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Movie movie, IFormFile? poster)
    {
        var existing = await _context.Movies.FindAsync(movie.Id);
        if (existing is null) return false;

        existing.Title = movie.Title;
        existing.Description = movie.Description;
        existing.Year = movie.Year;
        existing.Genre = movie.Genre;

        if (poster is { Length: > 0 })
        {
            DeletePosterFile(existing.PosterPath);
            existing.PosterPath = await SavePosterAsync(poster);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie is null) return false;

        DeletePosterFile(movie.PosterPath);
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    public string? ValidatePoster(IFormFile? poster)
    {
        if (poster is null || poster.Length == 0) return null;

        var ext = Path.GetExtension(poster.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            return "Дозволені формати постера: jpg, jpeg, png, webp, gif";

        if (poster.Length > MaxPosterSize)
            return "Розмір постера не повинен перевищувати 5 МБ";

        return null;
    }

    // ---------- робота з файлами постерів ----------

    private async Task<string> SavePosterAsync(IFormFile poster)
    {
        var folder = Path.Combine(_env.WebRootPath, PostersFolder);
        Directory.CreateDirectory(folder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(poster.FileName).ToLowerInvariant()}";
        var fullPath = Path.Combine(folder, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await poster.CopyToAsync(stream);

        return $"/{PostersFolder}/{fileName}";
    }

    private void DeletePosterFile(string? posterPath)
    {
        if (string.IsNullOrEmpty(posterPath)) return;

        var fullPath = Path.Combine(_env.WebRootPath, posterPath.TrimStart('/'));
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
