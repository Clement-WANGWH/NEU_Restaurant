using System.Linq.Expressions;
using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Models;

namespace NEU_Restaurant.Library.Services;

public class DataIntegrationService : IDataIntegrationService
{
    private readonly IDishStorage _dishStorage;
    private readonly IFavoriteStorage _favoriteStorage;

    public DataIntegrationService(IDishStorage dishStorage, IFavoriteStorage favoriteStorage)
    {
        _dishStorage = dishStorage;
        _favoriteStorage = favoriteStorage;
    }

    public async Task<IEnumerable<Item>> GetItemsAsync(
        Expression<Func<Dish, bool>> whereDish,
        Expression<Func<Favorite, bool>> whereFavorite,
        bool sortByRating)
    {
        var dishes = await _dishStorage.GetDishesAsync(whereDish);
        var favorites = await _favoriteStorage.GetFavoritesAsync(whereFavorite);

        var items = dishes.Select(dish => new Item
        {
            Id = dish.Id,
            Name = dish.Name,
            Price = (float)dish.Price,
            Canteen = dish.Canteen,
            Stall = dish.Stall,
            Flavor = dish.Flavor,
            DishRate = favorites.FirstOrDefault(fav => fav.DishId == dish.Id)?.DishRate ?? 0
        });

        // 根据用户需求进行排序
        if (sortByRating)
        {
            return items = items.OrderByDescending(item => item.DishRate)
                .ThenBy(item => item.Id);
        }
        else
        {
            return items = items.OrderBy(item => item.Id);
        }
    }

}