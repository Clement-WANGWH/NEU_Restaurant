using NEURestaurant.Server.Models;
using TheSalLab.GeneralReturnValues;

namespace DailyPoetryH.Server.Services;

public interface IDishRecommendService {
    Task<ServiceResult<RecommendResponse>> RecommendAsync(RecommendRequest request);
}

