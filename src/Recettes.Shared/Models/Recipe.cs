using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Recettes.Data.Models;

public class Recipe : Entity
{
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Instructions { get; set; }
    public IEnumerable<Ingredient>? Ingredients { get; set; }
    public string? ImageUrl { get; set; }
    public string? Notes { get; set; }

    [Range(1, 100)]
    public int Servings { get; set; }
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int TotalTime { get; set; }
    [IgnoreDataMember]
    public string SourceShort => Uri.TryCreate(Source, UriKind.Absolute, out var sourceUri) ? sourceUri.Authority : Source;
    public string Source { get; set; } = default!;
    public string? Url { get; set; }
}
