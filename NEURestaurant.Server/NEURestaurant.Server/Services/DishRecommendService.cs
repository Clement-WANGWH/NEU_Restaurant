using Microsoft.SemanticKernel.AI.ChatCompletion;
using NEURestaurant.Server.Models;
using TheSalLab.GeneralReturnValues;

namespace DailyPoetryH.Server.Services;

public class DishRecommendService : IDishRecommendService {
    private readonly IChatCompletion _chatCompletion;

    public DishRecommendService(
        IChatCompletionFactory chatCompletionFactory) {
        _chatCompletion = chatCompletionFactory.GetChatCompletion();
    }

    public async Task<ServiceResult<RecommendResponse>> RecommendAsync(RecommendRequest request) {
        var chatHistory = _chatCompletion.CreateNewChat();
        chatHistory.AddSystemMessage(@"
你是一个菜品推荐机器人。你根据时间、菜品信息、用户提示来推荐用户一个备选项中的菜品，请选择适合当前季节和时间的菜品。你的回答包括推荐的菜品的id和推荐理由两部分，以空格分隔。
===例子开始===
时间：2023年12月22日12:00
备选菜品：
红烧肉：id：0，口味：咸甜，食材：猪肉，描述：中餐、猪肉类菜品，适合冬天或寒冷的天气食用，因为它能够提供温暖和满足口腹之欲的感觉。
冷面：id：3，口味：酸甜，食材：面，猪肉，青菜，描述：中餐、主食类菜品、适合夏天或夜晚食用，因为它口感独特，既有凉爽的感觉，又有浓郁的味道。
用户提示：我想要吃点温暖的食物
响应：
0 红烧肉富含蛋白质、脂肪和碳水化合物，可以为身体提供充足的热量，帮助抵御寒冷，适合在冬天食用。
===例子结束===

===例子开始===
时间：2023年7月20日12:00
备选菜品：
红烧肉：id：0，口味：咸甜，食材：猪肉，描述：中餐、猪肉类菜品，适合冬天或寒冷的天气食用，因为它能够提供温暖和满足口腹之欲的感觉。
冷面：id：3，口味：酸甜，食材：面，猪肉，青菜，描述：中餐、主食类菜品、适合夏天或夜晚食用，因为它口感独特，既有凉爽的感觉，又有浓郁的味道。
用户提示：我想要吃点清爽的食物
响应：
3 冷面是一种清爽的食品，可以消除炎热的感觉，帮助人们降低体温，适合在夏天食用。
===例子结束===
            ");
        var now = DateTime.Now;
        var dishes = request.dishes;
        var prompt = request.prompt;

        string message = "时间：" + now.ToString("yyyy/MM/dd HH:mm") + "\n备选菜品:\n";

        for (var i = 0; i < dishes.Count; i++)
        {
            var dish = dishes[i];
            message += string.Format("{0}：id：{1}，口味：{2}，食材：{3}，描述：{4}\n", dish.Name, dish.Id, dish.Flavor, dish.Ingredient, dish.Description);
        }

        message += "用户提示：" + prompt;
        chatHistory.AddUserMessage(message);
        var reply = (await _chatCompletion.GenerateMessageAsync(chatHistory)).Split(' ');
        Console.Out.Write(reply);
        var recommend = new RecommendResponse();
        recommend.Id = int.Parse(reply[0]);
        recommend.Reason = reply[1];

        return ServiceResult<RecommendResponse>.CreateSucceededResult(recommend);
    }

}