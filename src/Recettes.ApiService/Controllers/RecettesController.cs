using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recettes.Data.Contexts;
using Recettes.Data.Models;

namespace Recettes.ServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecettesController(RecettesContext context) : ControllerBase
{

    // GET: api/Recipes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
    {
        return await context.Recipes
            .Include(x => x.Ingredients)
            .ToListAsync();

    }

    // GET: api/Recipes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetRecipe(long id)
    {
        var recipe = await context.Recipes
            .Include(x => x.Ingredients)
            .FirstOrDefaultAsync()
            ;

        if (recipe == null)
        {
            return NotFound();
        }

        return recipe;
    }

    // PUT: api/Recipes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRecipe(long id, Recipe recipe)
    {
        if (id != recipe.Id)
        {
            return BadRequest();
        }

        context.Entry(recipe).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RecipeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Recipes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
    {
        context.Recipes.Add(recipe);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
    }

    // DELETE: api/Recipes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(long id)
    {
        var recipe = await context.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }

        context.Recipes.Remove(recipe);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool RecipeExists(long id)
    {
        return context.Recipes.Any(e => e.Id == id);
    }
}