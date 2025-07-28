using System.ComponentModel.DataAnnotations;

namespace PlanificaChef_Web.Models;

public class ShoppingList
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsCompleted { get; set; }
    
    public decimal TotalEstimated { get; set; }
    
    public List<ShoppingListItem> Items { get; set; } = new();
}

public class ShoppingListItem
{
    public int Id { get; set; }
    
    public int ShoppingListId { get; set; }
    public ShoppingList ShoppingList { get; set; } = null!;
    
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    
    [MaxLength(50)]
    public string Quantity { get; set; } = string.Empty;
    
    public decimal EstimatedPrice { get; set; }
    
    public bool IsPurchased { get; set; }
}