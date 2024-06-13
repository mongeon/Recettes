using Microsoft.EntityFrameworkCore;
using Recettes.Data.Models;

namespace Recettes.Data.Contexts;

public class RecettesContext(DbContextOptions<RecettesContext> options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; }
}
