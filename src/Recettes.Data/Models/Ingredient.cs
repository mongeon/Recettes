using System.ComponentModel.DataAnnotations;

namespace Recettes.Data.Models;
public class Ingredient : Entity
{
    [Required]
    public required string Name { get; set; }
}
