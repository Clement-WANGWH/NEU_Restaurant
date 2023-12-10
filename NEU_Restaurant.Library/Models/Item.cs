namespace NEU_Restaurant.Library.Models;

public class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public float Price { get; set; }

    public string Canteen { get; set; } = string.Empty;

    public string Stall { get; set; } = string.Empty;

    public string Flavor { get; set; } = string.Empty;

    public int DishRate { get; set; }

}