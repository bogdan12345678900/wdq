using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.Controllers;

public class MoviesController : Controller
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<IActionResult> Index() =>
        View(await _movieService.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        return movie is null ? NotFound() : View(movie);
    }

    public IActionResult Create() => View(new Movie());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Movie movie, IFormFile? poster)
    {
        ValidatePoster(poster);
        if (!ModelState.IsValid) return View(movie);

        await _movieService.CreateAsync(movie, poster);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        return movie is null ? NotFound() : View(movie);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Movie movie, IFormFile? poster)
    {
        if (id != movie.Id) return NotFound();

        ValidatePoster(poster);
        if (!ModelState.IsValid) return View(movie);

        return await _movieService.UpdateAsync(movie, poster)
            ? RedirectToAction(nameof(Index))
            : NotFound();
    }

    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        return movie is null ? NotFound() : View(movie);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _movieService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private void ValidatePoster(IFormFile? poster)
    {
        var error = _movieService.ValidatePoster(poster);
        if (error is not null)
            ModelState.AddModelError("poster", error);
    }
}
