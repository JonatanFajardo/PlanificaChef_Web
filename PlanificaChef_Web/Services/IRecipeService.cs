using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Services;

public interface IRecipeService
{
    Task<List<Recipe>> GetAllRecipesAsync();
    Task<List<Recipe>> GetRecipesByMealTypeAsync(MealType mealType);
    Task<Recipe?> GetRecipeByIdAsync(int id);
    Task<Recipe> CreateRecipeAsync(Recipe recipe);
    Task<Recipe> UpdateRecipeAsync(Recipe recipe);
    Task DeleteRecipeAsync(int id);
    Task<decimal> CalculateRecipeTotalPriceAsync(int recipeId);
}