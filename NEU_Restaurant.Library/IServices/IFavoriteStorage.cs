using NEU_Restaurant.Library.Models;

namespace NEU_Restaurant.Library.IServices;

public interface IFavoriteStorage
{
	bool IsInitialized { get; }

	Task InitializeAsync();

	Task<Favorite> GetFavoriteAsync(int DishId);

	Task<IEnumerable<Favorite>> GetFavoritesAsync();

	Task SaveFavoriteAsync(Favorite favorite);
}