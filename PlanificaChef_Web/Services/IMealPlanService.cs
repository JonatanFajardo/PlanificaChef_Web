using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Services;

public interface IMealPlanService
{
    Task<List<MealPlan>> GetWeekMealPlanAsync(DateTime startDate);
    Task<MealPlan?> GetMealPlanByDateAsync(DateTime date);
    Task<MealPlan> CreateOrUpdateMealPlanAsync(DateTime date, int? breakfastId, int? lunchId, int? snackId, int? dinnerId);
    Task DeleteMealPlanAsync(int id);
    Task<decimal> CalculateDayTotalCostAsync(DateTime date);
    Task<decimal> CalculateWeekTotalCostAsync(DateTime startDate);
    Task<List<Ingredient>> GenerateShoppingListFromWeekPlanAsync(DateTime startDate);
}