using System.ComponentModel.DataAnnotations;

namespace MovieApp.Models;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Вкажіть назву")]
    [StringLength(200)]
    [Display(Name = "Назва")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Range(1888, 2100, ErrorMessage = "Некоректний рік")]
    [Display(Name = "Рік")]
    public int Year { get; set; } = DateTime.Now.Year;

    [StringLength(100)]
    [Display(Name = "Жанр")]
    public string? Genre { get; set; }

    [Display(Name = "Постер")]
    public string? PosterPath { get; set; }
}
