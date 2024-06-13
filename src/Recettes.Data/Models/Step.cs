using System.ComponentModel.DataAnnotations;

namespace Recettes.Data.Models;
public class Step : Entity
{
    [Required]
    public required string Description { get; set; }
}
