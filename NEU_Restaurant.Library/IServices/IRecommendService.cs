using NEU_Restaurant.Models;

namespace NEU_Restaurant.Library.IServices;

public interface IRecommendService
{
    Task<RecommendResponse> Recommend(RecommendRequest request);

}