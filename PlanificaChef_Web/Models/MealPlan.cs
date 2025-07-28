using System.ComponentModel.DataAnnotations;

namespace PlanificaChef_Web.Models;

public class MealPlan
{
    public int Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public int? BreakfastRecipeId { get; set; }
    public Recipe? BreakfastRecipe { get; set; }
    
    public int? LunchRecipeId { get; set; }
    public Recipe? LunchRecipe { get; set; }
    
    public int? SnackRecipeId { get; set; }
    public Recipe? SnackRecipe { get; set; }
    
    public int? DinnerRecipeId { get; set; }
    public Recipe? DinnerRecipe { get; set; }
    
    public decimal DayTotalCost { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}