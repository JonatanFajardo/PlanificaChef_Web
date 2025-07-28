using Microsoft.EntityFrameworkCore;
using PlanificaChef_Web.Data;
using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Services;

public class IngredientService : IIngredientService
{
    private readonly PlanificaChefContext _context;

    public IngredientService(PlanificaChefContext context)
    {
        _context = context;
    }

    public async Task<List<Ingredient>> GetAllIngredientsAsync()
    {
        return await _context.Ingredients
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<Ingredient?> GetIngredientByIdAsync(int id)
    {
        return await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
    {
        ingredient.CreatedAt = DateTime.UtcNow;
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
        return ingredient;
    }

    public async Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient)
    {
        _context.Entry(ingredient).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ingredient;
    }

    public async Task DeleteIngredientAsync(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient != null)
        {
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Ingredient>> SearchIngredientsByNameAsync(string name)
    {
        return await _context.Ingredients
            .Where(i => i.Name.Contains(name))
            .OrderBy(i => i.Name)
            .ToListAsync();
    }
}