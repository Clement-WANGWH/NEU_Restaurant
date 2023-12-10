using Microsoft.AspNetCore.Components;
using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Models;

namespace NEU_Restaurant.Pages;

public partial class Detail
{
	public Dish Dish { get; set; }

	public Favorite Favorite { get; set; }

    public string Symbol { get; set; } = string.Empty;

    //评分分数
    private double IconListValue { get; set; }

    //评分样式设置
    private List<string> IconList { get; } = new List<string>()
    {
        "fa-solid fa-face-sad-cry",
        "fa-solid fa-face-sad-tear",
        "fa-solid fa-face-smile",
        "fa-solid fa-face-surprise",
        "fa-solid fa-face-grin-stars"
    };

    private string GetIconList(int index) => IconList[index - 1];

    private async Task GetIconValueChanged()
    {
        if (SymbolChanged())
        {
            return;
        }
        await _favoriteStorage.SaveFavoriteAsync(new Favorite()
        {
            DishId = Dish.Id,
            DishRate = (int)IconListValue
        });
        StateHasChanged();
    }

    private bool SymbolChanged()
    {
        if (IconListValue == 0)
        {
            Symbol = "未评分";
            return true;
        }
        Symbol = (IconListValue - 1) switch
        {
            0 => "反胃",
            1 => "难吃",
            2 => "一般",
            3 => "不错",
            _ => "惊喜"
        };
        return false;
    }
    
    /*
    private string GetIconValueChanged() => (IconListValue - 1) switch
    {
        -1 => "未评分",
        0 => "反胃",
        1 => "难吃",
        2 => "一般",
        3 => "不错",
        _ => "惊喜"
    };*/

    protected override async Task OnInitializedAsync()
	{
		Dish = await _dishStorage.GetDishAsync(DishId);
		Favorite = await _favoriteStorage.GetFavoriteAsync(DishId);
        IconListValue = Favorite?.DishRate ?? 0;
        SymbolChanged();
    }
}