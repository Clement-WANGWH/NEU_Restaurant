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

    public async Task<IEnumerable<Item>> GetRatedItemsAsync(
        Expression<Func<Dish, bool>> whereDish,
        Expression<Func<Favorite, bool>> whereFavorite,
        bool sortByRating)
    {
        var dishes = await _dishStorage.GetDishesAsync(whereDish);
        var favorites = await _favoriteStorage.GetFavoritesAsync(whereFavorite);

        var items = favorites.Select(favorite => new Item
        {
            Id = favorite.DishId,
            Name = dishes.FirstOrDefault(dish => dish.Id == favorite.DishId)?.Name ?? "未知菜品",
            Price = (float)dishes.FirstOrDefault(dish => dish.Id == favorite.DishId).Price,
            Canteen = dishes.FirstOrDefault(dish => dish.Id == favorite.DishId).Canteen,
            Stall = dishes.FirstOrDefault(dish => dish.Id == favorite.DishId).Stall,
            Flavor = dishes.FirstOrDefault(dish => dish.Id == favorite.DishId).Flavor,
            DishRate = favorite.DishRate
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

    public Task<IEnumerable<string>> GetCanteensAsync(Expression<Func<Dish, bool>> whereDish)
    {
        var front = new List<string>() { "全部食堂" };
        return _dishStorage.GetDishesAsync(whereDish).ContinueWith(dishes =>
        {
            var canteens = dishes.Result.Select(dish => dish.Canteen).Distinct();
            return front.Concat(canteens);
        });
    }

    //生成餐厅与stall对应的map
    public async Task<Dictionary<string, IEnumerable<string>>> GetCanteenStallMapAsync(Expression<Func<Dish, bool>> whereDish)
    {
        var canteenStallMap = new Dictionary<string, IEnumerable<string>>();
        var canteens = await GetCanteensAsync(whereDish);
        foreach (var canteen in canteens)
        {
            var stalls = await GetStallsAsync(whereDish, canteen);
            canteenStallMap[canteen] = stalls;
        }

        return canteenStallMap;
    }

    public Task<IEnumerable<string>> GetStallsAsync(Expression<Func<Dish, bool>> whereDish, string canteen)
    {
        var front = new List<string>() { "全部档口" };
        return _dishStorage.GetDishesAsync(whereDish).ContinueWith(dishes =>
        {
            var stalls = dishes.Result.Where(dish => dish.Canteen == canteen).Select(dish => dish.Stall).Distinct();
            return front.Concat(stalls);
        });
    }

    public Task<IEnumerable<string>> GetFlavorsAsync(Expression<Func<Dish, bool>> whereDish)
    {
        var front = new List<string>() { "全部口味" };
        return _dishStorage.GetDishesAsync(whereDish).ContinueWith(dishes =>
        {
            var flavors = dishes.Result.Select(dish => dish.Flavor).Distinct();
            return front.Concat(flavors);
        });
    }

    public Task<IEnumerable<string>> GetNameAndFlavorAsync(Expression<Func<Dish, bool>> whereDish)
    {
        return _dishStorage.GetDishesAsync(whereDish).ContinueWith(dishes =>
        {
            var name = dishes.Result.Select(dish => dish.Name ).Distinct();
            var ingredientList = dishes.Result
                .Select(dish => dish.Ingredient)
                .SelectMany(ingredients => ingredients.Split('、'))
                .Select(ingredient => ingredient.Trim()) // 去掉空格
                .Distinct()
                .ToList();
            return name.Concat(ingredientList);
        });
    }
}