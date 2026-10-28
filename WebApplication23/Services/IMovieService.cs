using MovieApp.Models;

namespace MovieApp.Services;

public interface IMovieService
{
    Task<List<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task CreateAsync(Movie movie, IFormFile? poster);
    Task<bool> UpdateAsync(Movie movie, IFormFile? poster);
    Task<bool> DeleteAsync(int id);

    /// <summary>Повертає текст помилки або null, якщо файл постера коректний (чи не вказаний).</summary>
    string? ValidatePoster(IFormFile? poster);
}
