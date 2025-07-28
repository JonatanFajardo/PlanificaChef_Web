using Microsoft.EntityFrameworkCore;
using PlanificaChef_Web.Data;
using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Services;

public class RecipeService : IRecipeService
{
    private readonly PlanificaChefContext _context;

    public RecipeService(PlanificaChefContext context)
    {
        _context = context;
    }

    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .ToListAsync();
    }

    public async Task<List<Recipe>> GetRecipesByMealTypeAsync(MealType mealType)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .Where(r => r.MealType == mealType)
            .ToListAsync();
    }

    public async Task<Recipe?> GetRecipeByIdAsync(int id)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        recipe.TotalPrice = recipe.Ingredients.Sum(ri => ri.Price);
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task<Recipe> UpdateRecipeAsync(Recipe recipe)
    {
        recipe.TotalPrice = recipe.Ingredients.Sum(ri => ri.Price);
        _context.Entry(recipe).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task DeleteRecipeAsync(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe != null)
        {
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<decimal> CalculateRecipeTotalPriceAsync(int recipeId)
    {
        var ingredients = await _context.RecipeIngredients
            .Where(ri => ri.RecipeId == recipeId)
            .ToListAsync();

        return ingredients.Sum(ri => ri.Price);
    }
}