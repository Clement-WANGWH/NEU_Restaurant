using System.Linq.Expressions;
using NEU_Restaurant.Library.Models;

namespace NEU_Restaurant.Library.IServices;

public interface IFavoriteStorage
{
	bool IsInitialized { get; }

	Task InitializeAsync();

	Task<Favorite> GetFavoriteAsync(int DishId);

	Task<IEnumerable<Favorite>> GetFavoritesAsync(Expression<Func<Favorite,bool>> where);

	Task SaveFavoriteAsync(Favorite favorite);
}