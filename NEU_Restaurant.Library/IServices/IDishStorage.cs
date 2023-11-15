using System.Linq.Expressions;
using NEU_Restaurant.Library.Models;

namespace NEU_Restaurant.Library.IServices;

public interface IDishStorage
{
	bool IsInitialized { get; }

	Task InitializeAsync();

	Task<Dish> GetDishAsync(int id);

	Task<IEnumerable<Dish>> GetDishesAsync(Expression<Func<Dish, bool>> where, int skip, int take);
}