using Recettes.Data.Models;

namespace Recettes.Web;

public class RecettesApiClient(HttpClient httpClient)
{
    public async Task<Recipe[]> GetRecipesAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<Recipe>? recipes = null;

        await foreach (var recipe in httpClient.GetFromJsonAsAsyncEnumerable<Recipe>("Recettes", cancellationToken))
        {
            if (recipes?.Count >= maxItems)
            {
                break;
            }
            if (recipe is not null)
            {
                recipes ??= [];
                recipes.Add(recipe);
            }
        }

        return recipes?.ToArray() ?? [];
    }
    public async Task<Recipe?> GetRecipeAsync(long id, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Recipe>($"Recettes/{id}", cancellationToken);
    }
}