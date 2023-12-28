using System.Data.SqlTypes;

namespace NEURestaurant.Server.Models;

public class Dish
{
    // 菜品id
    public int Id { get; set; }
    // 菜品名称
    public string Name { get; set; }
    // 价格
    public float Price { get; set; }
    // 口味
    public string Flavor { get; set; }
    // 食堂
    public string Canteen { get; set; } = string.Empty;
    // 窗口
    public string Stall { get; set; } = string.Empty;
    // 食材，如牛肉、拉面
    public string Ingredient { get; set; }
    // 图片
    public string Image { get; set; } = string.Empty;
    // 描述
    public string Description { get; set; }
}