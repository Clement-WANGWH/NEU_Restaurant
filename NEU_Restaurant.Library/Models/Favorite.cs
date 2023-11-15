using SQLite;

namespace NEU_Restaurant.Library.Models;

public class Favorite
{
	[PrimaryKey]
	public int PoetryId { get; set; }

	public bool IsFavorite { get; set; }

	public bool IsDisliked { get; set; }

	public long Timestamp { get; set; }
}