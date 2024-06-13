using System.ComponentModel.DataAnnotations;

namespace Recettes.Data.Models;

public class Recipe
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Instructions { get; set; }
    public string? Ingredients { get; set; }
    public string? ImageUrl { get; set; }
    public string? Notes { get; set; }
    public int Servings { get; set; }
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int TotalTime { get; set; }
    public string? Source { get; set; }
    public string? Url { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
