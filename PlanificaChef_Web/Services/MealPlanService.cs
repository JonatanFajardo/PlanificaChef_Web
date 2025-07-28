using Microsoft.EntityFrameworkCore;
using PlanificaChef_Web.Data;
using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Services;

public class MealPlanService : IMealPlanService
{
    private readonly PlanificaChefContext _context;

    public MealPlanService(PlanificaChefContext context)
    {
        _context = context;
    }

    public async Task<List<MealPlan>> GetWeekMealPlanAsync(DateTime startDate)
    {
        var endDate = startDate.AddDays(7);
        
        return await _context.MealPlans
            .Include(mp => mp.BreakfastRecipe)
            .Include(mp => mp.LunchRecipe)
            .Include(mp => mp.SnackRecipe)
            .Include(mp => mp.DinnerRecipe)
            .Where(mp => mp.Date >= startDate && mp.Date < endDate)
            .OrderBy(mp => mp.Date)
            .ToListAsync();
    }

    public async Task<MealPlan?> GetMealPlanByDateAsync(DateTime date)
    {
        return await _context.MealPlans
            .Include(mp => mp.BreakfastRecipe)
            .Include(mp => mp.LunchRecipe)
            .Include(mp => mp.SnackRecipe)
            .Include(mp => mp.DinnerRecipe)
            .FirstOrDefaultAsync(mp => mp.Date.Date == date.Date);
    }

    public async Task<MealPlan> CreateOrUpdateMealPlanAsync(DateTime date, int? breakfastId, int? lunchId, int? snackId, int? dinnerId)
    {
        var existingPlan = await GetMealPlanByDateAsync(date);
        
        if (existingPlan != null)
        {
            existingPlan.BreakfastRecipeId = breakfastId;
            existingPlan.LunchRecipeId = lunchId;
            existingPlan.SnackRecipeId = snackId;
            existingPlan.DinnerRecipeId = dinnerId;
            existingPlan.DayTotalCost = await CalculateDayTotalCostForPlanAsync(existingPlan);
            
            await _context.SaveChangesAsync();
            return existingPlan;
        }
        else
        {
            var newPlan = new MealPlan
            {
                Date = date.Date,
                BreakfastRecipeId = breakfastId,
                LunchRecipeId = lunchId,
                SnackRecipeId = snackId,
                DinnerRecipeId = dinnerId
            };
            
            newPlan.DayTotalCost = await CalculateDayTotalCostForPlanAsync(newPlan);
            
            _context.MealPlans.Add(newPlan);
            await _context.SaveChangesAsync();
            return newPlan;
        }
    }

    public async Task DeleteMealPlanAsync(int id)
    {
        var mealPlan = await _context.MealPlans.FindAsync(id);
        if (mealPlan != null)
        {
            _context.MealPlans.Remove(mealPlan);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<decimal> CalculateDayTotalCostAsync(DateTime date)
    {
        var mealPlan = await GetMealPlanByDateAsync(date);
        if (mealPlan == null) return 0;

        return await CalculateDayTotalCostForPlanAsync(mealPlan);
    }

    public async Task<decimal> CalculateWeekTotalCostAsync(DateTime startDate)
    {
        var weekPlans = await GetWeekMealPlanAsync(startDate);
        return weekPlans.Sum(mp => mp.DayTotalCost);
    }

    public async Task<List<Ingredient>> GenerateShoppingListFromWeekPlanAsync(DateTime startDate)
    {
        var weekPlans = await GetWeekMealPlanAsync(startDate);
        var ingredientsList = new List<Ingredient>();

        foreach (var plan in weekPlans)
        {
            await AddRecipeIngredientsToList(plan.BreakfastRecipeId, ingredientsList);
            await AddRecipeIngredientsToList(plan.LunchRecipeId, ingredientsList);
            await AddRecipeIngredientsToList(plan.SnackRecipeId, ingredientsList);
            await AddRecipeIngredientsToList(plan.DinnerRecipeId, ingredientsList);
        }

        return ingredientsList.Distinct().ToList();
    }

    private async Task<decimal> CalculateDayTotalCostForPlanAsync(MealPlan mealPlan)
    {
        decimal total = 0;

        if (mealPlan.BreakfastRecipeId.HasValue)
        {
            var breakfast = await _context.Recipes.FindAsync(mealPlan.BreakfastRecipeId.Value);
            total += breakfast?.TotalPrice ?? 0;
        }

        if (mealPlan.LunchRecipeId.HasValue)
        {
            var lunch = await _context.Recipes.FindAsync(mealPlan.LunchRecipeId.Value);
            total += lunch?.TotalPrice ?? 0;
        }

        if (mealPlan.SnackRecipeId.HasValue)
        {
            var snack = await _context.Recipes.FindAsync(mealPlan.SnackRecipeId.Value);
            total += snack?.TotalPrice ?? 0;
        }

        if (mealPlan.DinnerRecipeId.HasValue)
        {
            var dinner = await _context.Recipes.FindAsync(mealPlan.DinnerRecipeId.Value);
            total += dinner?.TotalPrice ?? 0;
        }

        return total;
    }

    private async Task AddRecipeIngredientsToList(int? recipeId, List<Ingredient> ingredientsList)
    {
        if (!recipeId.HasValue) return;

        var recipeIngredients = await _context.RecipeIngredients
            .Include(ri => ri.Ingredient)
            .Where(ri => ri.RecipeId == recipeId.Value)
            .Select(ri => ri.Ingredient)
            .ToListAsync();

        ingredientsList.AddRange(recipeIngredients);
    }
}