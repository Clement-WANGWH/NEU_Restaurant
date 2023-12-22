using NEU_Restaurant.Library.Models;
using System.Linq.Expressions;

namespace NEU_Restaurant.Library.IServices;

public interface IDataIntegrationService
{
    Task <IEnumerable<Item>> GetItemsAsync(Expression<Func<Dish, bool>> whereDish,
        Expression<Func<Favorite, bool>> whereFavorite,
        bool sortByRating);

    Task <IEnumerable<Item>> GetRatedItemsAsync(Expression<Func<Dish, bool>> whereDish,
        Expression<Func<Favorite, bool>> whereFavorite,
        bool sortByRating);

    Task <IEnumerable<String>> GetCanteensAsync(Expression<Func<Dish, bool>> whereDish);

    Task <Dictionary<string, IEnumerable<string>>> GetCanteenStallMapAsync(Expression<Func<Dish, bool>> whereDish);

    Task <IEnumerable<string>> GetFlavorsAsync(Expression<Func<Dish, bool>> whereDish);
}