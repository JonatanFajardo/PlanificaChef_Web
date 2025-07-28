using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Services;

public interface IShoppingListService
{
    Task<List<ShoppingList>> GetAllShoppingListsAsync();
    Task<ShoppingList?> GetShoppingListByIdAsync(int id);
    Task<ShoppingList?> GetActiveShoppingListAsync();
    Task<ShoppingList> CreateShoppingListAsync(string name);
    Task<ShoppingList> UpdateShoppingListAsync(ShoppingList shoppingList);
    Task DeleteShoppingListAsync(int id);
    Task<ShoppingListItem> AddItemToListAsync(int shoppingListId, int ingredientId, string quantity, decimal estimatedPrice);
    Task RemoveItemFromListAsync(int itemId);
    Task MarkItemAsPurchasedAsync(int itemId);
    Task<decimal> CalculateListTotalAsync(int shoppingListId);
    Task<ShoppingList> CreateNewActiveShoppingListAsync(string name);
}