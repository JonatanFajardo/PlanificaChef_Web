using System.ComponentModel.DataAnnotations;

namespace PlanificaChef_Web.Models;

public class Recipe
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public MealType MealType { get; set; }
    
    public int PreparationTimeMinutes { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<RecipeIngredient> Ingredients { get; set; } = new();
}

public enum MealType
{
    Breakfast,
    Lunch,
    Snack,
    Dinner
}