using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Services;

public interface IIngredientService
{
    Task<List<Ingredient>> GetAllIngredientsAsync();
    Task<Ingredient?> GetIngredientByIdAsync(int id);
    Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);
    Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient);
    Task DeleteIngredientAsync(int id);
    Task<List<Ingredient>> SearchIngredientsByNameAsync(string name);
}