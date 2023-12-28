using DailyPoetryH.Server.Commands;
using DailyPoetryH.Server.Services;
using Microsoft.AspNetCore.Mvc;
using NEURestaurant.Server.Models;
using TheSalLab.GeneralReturnValues;

namespace DailyPoetryH.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController {
    private readonly IDishRecommendService _dishRecommendService;

    public HomeController(IDishRecommendService dishRecommendService) {
        _dishRecommendService = dishRecommendService;
    }

    [Route("dish-recommend")]
    [HttpPost]
    public async Task<ServiceResultViewModel<RecommendResponse>> DishRecommendAsync(
        [FromBody] RecommendRequest request)
    {
        var result = await _dishRecommendService.RecommendAsync(request);
        return result.ToServiceResultViewModel();
    }

}