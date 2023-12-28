namespace NEURestaurant.Server.Models;

public class RecommendRequest
{
    public List<Dish> dishes { get; set; }
    public String prompt { get; set; }
}