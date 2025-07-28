using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlanificaChef_Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MealType = table.Column<int>(type: "int", nullable: false),
                    PreparationTimeMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    TotalEstimated = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BreakfastRecipeId = table.Column<int>(type: "int", nullable: true),
                    LunchRecipeId = table.Column<int>(type: "int", nullable: true),
                    SnackRecipeId = table.Column<int>(type: "int", nullable: true),
                    DinnerRecipeId = table.Column<int>(type: "int", nullable: true),
                    DayTotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlans_Recipes_BreakfastRecipeId",
                        column: x => x.BreakfastRecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MealPlans_Recipes_DinnerRecipeId",
                        column: x => x.DinnerRecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MealPlans_Recipes_LunchRecipeId",
                        column: x => x.LunchRecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MealPlans_Recipes_SnackRecipeId",
                        column: x => x.SnackRecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => new { x.RecipeId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingListId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "BasePrice", "CreatedAt", "Name", "Unit" },
                values: new object[,]
                {
                    { 1, 12.50m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9263), "Huevos", "pieza" },
                    { 2, 25.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9270), "Leche", "litro" },
                    { 3, 30.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9272), "Pan", "paquete" },
                    { 4, 80.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9274), "Pollo", "kg" },
                    { 5, 35.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9277), "Arroz", "kg" },
                    { 6, 15.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9279), "Yogurt", "pieza" },
                    { 7, 45.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9282), "Granola", "paquete" },
                    { 8, 20.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9283), "Avena", "kg" },
                    { 9, 50.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9285), "Frutas", "kg" },
                    { 10, 30.00m, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9287), "Verduras", "kg" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CreatedAt", "MealType", "Name", "PreparationTimeMinutes", "TotalPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9935), 0, "Huevos Revueltos", 15, 45.50m },
                    { 2, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9944), 1, "Pollo al Curry", 45, 85.00m },
                    { 3, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9946), 2, "Yogurt con Granola", 5, 22.50m },
                    { 4, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9949), 3, "Sopa de Verduras", 30, 33.00m },
                    { 5, new DateTime(2025, 7, 27, 2, 9, 41, 822, DateTimeKind.Utc).AddTicks(9951), 0, "Avena con Frutas", 10, 28.75m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_BreakfastRecipeId",
                table: "MealPlans",
                column: "BreakfastRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_DinnerRecipeId",
                table: "MealPlans",
                column: "DinnerRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_LunchRecipeId",
                table: "MealPlans",
                column: "LunchRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_SnackRecipeId",
                table: "MealPlans",
                column: "SnackRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_IngredientId",
                table: "ShoppingListItems",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_ShoppingListId",
                table: "ShoppingListItems",
                column: "ShoppingListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "ShoppingLists");
        }
    }
}
