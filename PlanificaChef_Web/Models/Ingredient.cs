using System.ComponentModel.DataAnnotations;

namespace PlanificaChef_Web.Models;

public class Ingredient
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public decimal BasePrice { get; set; }
    
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class RecipeIngredient
{
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
    
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    
    [MaxLength(50)]
    public string Quantity { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
}