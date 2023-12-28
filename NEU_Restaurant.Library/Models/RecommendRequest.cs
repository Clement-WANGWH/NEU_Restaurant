using NEU_Restaurant.Library.Models;

namespace NEU_Restaurant.Models;

public class RecommendRequest
{
    public List<Item> dishes { get; set; }
    public String prompt { get; set; }
}