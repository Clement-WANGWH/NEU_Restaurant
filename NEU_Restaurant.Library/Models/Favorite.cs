using SQLite;

namespace NEU_Restaurant.Library.Models;

public class Favorite
{
	[PrimaryKey]
	public int DishId { get; set; }

	public int DishRate { get; set; }

	public long Timestamp { get; set; }
}