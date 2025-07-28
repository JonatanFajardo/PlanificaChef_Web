using Microsoft.EntityFrameworkCore;
using PlanificaChef_Web.Data;
using PlanificaChef_Web.Models;

namespace PlanificaChef_Web.Services;

public class ShoppingListService : IShoppingListService
{
    private readonly PlanificaChefContext _context;

    public ShoppingListService(PlanificaChefContext context)
    {
        _context = context;
    }

    public async Task<List<ShoppingList>> GetAllShoppingListsAsync()
    {
        return await _context.ShoppingLists
            .Include(sl => sl.Items)
                .ThenInclude(sli => sli.Ingredient)
            .OrderByDescending(sl => sl.CreatedAt)
            .ToListAsync();
    }

    public async Task<ShoppingList?> GetShoppingListByIdAsync(int id)
    {
        return await _context.ShoppingLists
            .Include(sl => sl.Items)
                .ThenInclude(sli => sli.Ingredient)
            .FirstOrDefaultAsync(sl => sl.Id == id);
    }

    public async Task<ShoppingList?> GetActiveShoppingListAsync()
    {
        return await _context.ShoppingLists
            .Include(sl => sl.Items)
                .ThenInclude(sli => sli.Ingredient)
            .FirstOrDefaultAsync(sl => !sl.IsCompleted);
    }

    public async Task<ShoppingList> CreateShoppingListAsync(string name)
    {
        var shoppingList = new ShoppingList
        {
            Name = name,
            CreatedAt = DateTime.UtcNow
        };

        _context.ShoppingLists.Add(shoppingList);
        await _context.SaveChangesAsync();
        return shoppingList;
    }

    public async Task<ShoppingList> UpdateShoppingListAsync(ShoppingList shoppingList)
    {
        shoppingList.TotalEstimated = shoppingList.Items.Sum(item => item.EstimatedPrice);
        _context.Entry(shoppingList).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return shoppingList;
    }

    public async Task DeleteShoppingListAsync(int id)
    {
        var shoppingList = await _context.ShoppingLists.FindAsync(id);
        if (shoppingList != null)
        {
            _context.ShoppingLists.Remove(shoppingList);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ShoppingListItem> AddItemToListAsync(int shoppingListId, int ingredientId, string quantity, decimal estimatedPrice)
    {
        var item = new ShoppingListItem
        {
            ShoppingListId = shoppingListId,
            IngredientId = ingredientId,
            Quantity = quantity,
            EstimatedPrice = estimatedPrice
        };

        _context.ShoppingListItems.Add(item);
        await _context.SaveChangesAsync();

        await UpdateListTotalAsync(shoppingListId);
        return item;
    }

    public async Task RemoveItemFromListAsync(int itemId)
    {
        var item = await _context.ShoppingListItems.FindAsync(itemId);
        if (item != null)
        {
            var shoppingListId = item.ShoppingListId;
            _context.ShoppingListItems.Remove(item);
            await _context.SaveChangesAsync();
            await UpdateListTotalAsync(shoppingListId);
        }
    }

    public async Task MarkItemAsPurchasedAsync(int itemId)
    {
        var item = await _context.ShoppingListItems.FindAsync(itemId);
        if (item != null)
        {
            item.IsPurchased = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<decimal> CalculateListTotalAsync(int shoppingListId)
    {
        return await _context.ShoppingListItems
            .Where(sli => sli.ShoppingListId == shoppingListId)
            .SumAsync(sli => sli.EstimatedPrice);
    }

    public async Task<ShoppingList> CreateNewActiveShoppingListAsync(string name)
    {
        // Marcar todas las listas existentes como completadas
        var activeLists = await _context.ShoppingLists
            .Where(sl => !sl.IsCompleted)
            .ToListAsync();
            
        foreach (var list in activeLists)
        {
            list.IsCompleted = true;
        }

        // Crear nueva lista activa
        var newList = new ShoppingList
        {
            Name = name,
            CreatedAt = DateTime.UtcNow,
            IsCompleted = false
        };

        _context.ShoppingLists.Add(newList);
        await _context.SaveChangesAsync();
        return newList;
    }

    private async Task UpdateListTotalAsync(int shoppingListId)
    {
        var shoppingList = await _context.ShoppingLists.FindAsync(shoppingListId);
        if (shoppingList != null)
        {
            shoppingList.TotalEstimated = await CalculateListTotalAsync(shoppingListId);
            await _context.SaveChangesAsync();
        }
    }
}