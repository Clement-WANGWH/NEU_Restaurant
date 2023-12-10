using SQLite;

namespace NEU_Restaurant.Library.Models;

[SQLite.Table("dishes")]

public class Dish
{
	[SQLite.Column("id")]
	[PrimaryKey]
	public int Id { get; set; }

	[SQLite.Column("name")]
	public string Name { get; set; } = string.Empty;

	[SQLite.Column("price")]
	public float Price { get; set; }

	[SQLite.Column("canteen")]
	public string Canteen { get; set; } = string.Empty;

    [SQLite.Column("stall")] 
    public string Stall { get; set; } = string.Empty;

	[SQLite.Column("flavor")]
	public string Flavor { get; set; } = string.Empty;

	[SQLite.Column("ingredient")]
	public string Ingredient { get; set; } = string.Empty;

	[SQLite.Column("image")]
	public string Image { get; set; } = string.Empty;

	[SQLite.Column("description")]
	public string Description { get; set; } = string.Empty;

	private string _snippet;

	[SQLite.Ignore]
	public string Snippet => _snippet ??= Description.Split('。')[0].Replace("\r\n", " ");

}