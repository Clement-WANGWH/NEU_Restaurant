using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Models;
using System.Net.Http;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Headers;
using DailyPoetryH.Library.Services;
using TheSalLab.GeneralReturnValues;

namespace NEU_Restaurant.Library.Services;

public class RecommendService : IRecommendService
{

    private readonly IAlertService _alertService;

    private const string Server = "菜品推荐服务器";

    public async Task<RecommendResponse> Recommend(RecommendRequest request)
    {
        using var httpClient = new HttpClient();

        var jsonString = JsonSerializer.Serialize(request);
        var content =
            JsonContent.Create(request, typeof(RecommendRequest), new MediaTypeHeaderValue("application/json"));

        HttpResponseMessage response;
        try
        {
            response =
                await httpClient.PostAsync(
                    "http://127.0.0.1:9988/Home/dish-recommend", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            await _alertService.AlertAsync(ErrorMessages.HttpClientErrorTitle,
                ErrorMessages.GetHttpClientError(Server, e.Message),
                ErrorMessages.HttpClientErrorButton);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ServiceResultViewModel<RecommendResponse>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            });

        return result.Result;;
    }
}