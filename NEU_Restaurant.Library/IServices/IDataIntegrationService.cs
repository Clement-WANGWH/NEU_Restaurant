using NEU_Restaurant.Library.Models;
using System.Linq.Expressions;

namespace NEU_Restaurant.Library.IServices;

public interface IDataIntegrationService
{
    Task <IEnumerable<Item>> GetItemsAsync(Expression<Func<Dish, bool>> whereDish,
        Expression<Func<Favorite, bool>> whereFavorite,
        bool sortByRating);

}