using Microsoft.EntityFrameworkCore;
using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Data;

public class PlanificaChefContext : DbContext
{
    public PlanificaChefContext(DbContextOptions<PlanificaChefContext> options) : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    public DbSet<MealPlan> MealPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // RecipeIngredient composite key
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecipeId);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany()
            .HasForeignKey(ri => ri.IngredientId);

        // ShoppingListItem relationships
        modelBuilder.Entity<ShoppingListItem>()
            .HasOne(sli => sli.ShoppingList)
            .WithMany(sl => sl.Items)
            .HasForeignKey(sli => sli.ShoppingListId);

        modelBuilder.Entity<ShoppingListItem>()
            .HasOne(sli => sli.Ingredient)
            .WithMany()
            .HasForeignKey(sli => sli.IngredientId);

        // MealPlan relationships
        modelBuilder.Entity<MealPlan>()
            .HasOne(mp => mp.BreakfastRecipe)
            .WithMany()
            .HasForeignKey(mp => mp.BreakfastRecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MealPlan>()
            .HasOne(mp => mp.LunchRecipe)
            .WithMany()
            .HasForeignKey(mp => mp.LunchRecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MealPlan>()
            .HasOne(mp => mp.SnackRecipe)
            .WithMany()
            .HasForeignKey(mp => mp.SnackRecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MealPlan>()
            .HasOne(mp => mp.DinnerRecipe)
            .WithMany()
            .HasForeignKey(mp => mp.DinnerRecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed ingredients
        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { Id = 1, Name = "Huevos", BasePrice = 12.50m, Unit = "pieza" },
            new Ingredient { Id = 2, Name = "Leche", BasePrice = 25.00m, Unit = "litro" },
            new Ingredient { Id = 3, Name = "Pan", BasePrice = 30.00m, Unit = "paquete" },
            new Ingredient { Id = 4, Name = "Pollo", BasePrice = 80.00m, Unit = "kg" },
            new Ingredient { Id = 5, Name = "Arroz", BasePrice = 35.00m, Unit = "kg" },
            new Ingredient { Id = 6, Name = "Yogurt", BasePrice = 15.00m, Unit = "pieza" },
            new Ingredient { Id = 7, Name = "Granola", BasePrice = 45.00m, Unit = "paquete" },
            new Ingredient { Id = 8, Name = "Avena", BasePrice = 20.00m, Unit = "kg" },
            new Ingredient { Id = 9, Name = "Frutas", BasePrice = 50.00m, Unit = "kg" },
            new Ingredient { Id = 10, Name = "Verduras", BasePrice = 30.00m, Unit = "kg" }
        );

        // Seed recipes
        modelBuilder.Entity<Recipe>().HasData(
            new Recipe { Id = 1, Name = "Huevos Revueltos", MealType = MealType.Breakfast, PreparationTimeMinutes = 15, TotalPrice = 45.50m },
            new Recipe { Id = 2, Name = "Pollo al Curry", MealType = MealType.Lunch, PreparationTimeMinutes = 45, TotalPrice = 85.00m },
            new Recipe { Id = 3, Name = "Yogurt con Granola", MealType = MealType.Snack, PreparationTimeMinutes = 5, TotalPrice = 22.50m },
            new Recipe { Id = 4, Name = "Sopa de Verduras", MealType = MealType.Dinner, PreparationTimeMinutes = 30, TotalPrice = 33.00m },
            new Recipe { Id = 5, Name = "Avena con Frutas", MealType = MealType.Breakfast, PreparationTimeMinutes = 10, TotalPrice = 28.75m }
        );

        // Seed recipe ingredients
        modelBuilder.Entity<RecipeIngredient>().HasData(
            // Huevos Revueltos
            new RecipeIngredient { RecipeId = 1, IngredientId = 1, Quantity = "3", Price = 12.50m },
            new RecipeIngredient { RecipeId = 1, IngredientId = 2, Quantity = "100ml", Price = 8.00m },
            new RecipeIngredient { RecipeId = 1, IngredientId = 3, Quantity = "2 rebanadas", Price = 5.00m },
            
            // Pollo al Curry
            new RecipeIngredient { RecipeId = 2, IngredientId = 4, Quantity = "500g", Price = 40.00m },
            new RecipeIngredient { RecipeId = 2, IngredientId = 5, Quantity = "200g", Price = 14.00m },
            new RecipeIngredient { RecipeId = 2, IngredientId = 10, Quantity = "200g", Price = 15.00m },
            
            // Yogurt con Granola
            new RecipeIngredient { RecipeId = 3, IngredientId = 6, Quantity = "1", Price = 15.00m },
            new RecipeIngredient { RecipeId = 3, IngredientId = 7, Quantity = "50g", Price = 7.50m },
            
            // Sopa de Verduras
            new RecipeIngredient { RecipeId = 4, IngredientId = 10, Quantity = "300g", Price = 18.00m },
            new RecipeIngredient { RecipeId = 4, IngredientId = 5, Quantity = "100g", Price = 7.00m },
            
            // Avena con Frutas
            new RecipeIngredient { RecipeId = 5, IngredientId = 8, Quantity = "100g", Price = 8.00m },
            new RecipeIngredient { RecipeId = 5, IngredientId = 9, Quantity = "150g", Price = 15.00m },
            new RecipeIngredient { RecipeId = 5, IngredientId = 2, Quantity = "200ml", Price = 5.75m }
        );
    }
}