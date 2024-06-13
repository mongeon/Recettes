using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recettes.Data.Contexts;
using Recettes.Data.Models;

namespace Recettes.ServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecettesController(RecettesContext context) : ControllerBase
{

    // GET: api/SupportTickets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetTickets()
    {
        return await context.Recipes.ToListAsync();
    }

    // GET: api/SupportTickets/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetSupportTicket(int id)
    {
        var supportTicket = await context.Recipes.FindAsync(id);

        if (supportTicket == null)
        {
            return NotFound();
        }

        return supportTicket;
    }

    // PUT: api/SupportTickets/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSupportTicket(int id, Recipe supportTicket)
    {
        if (id != supportTicket.Id)
        {
            return BadRequest();
        }

        context.Entry(supportTicket).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SupportTicketExists(id))
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

    // POST: api/SupportTickets
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Recipe>> PostSupportTicket(Recipe supportTicket)
    {
        context.Recipes.Add(supportTicket);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetSupportTicket", new { id = supportTicket.Id }, supportTicket);
    }

    // DELETE: api/SupportTickets/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupportTicket(int id)
    {
        var supportTicket = await context.Recipes.FindAsync(id);
        if (supportTicket == null)
        {
            return NotFound();
        }

        context.Recipes.Remove(supportTicket);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool SupportTicketExists(int id)
    {
        return context.Recipes.Any(e => e.Id == id);
    }
}