using System.Linq.Expressions;
using NEU_Restaurant.Library.Models;
using Microsoft.AspNetCore.Components;

namespace NEU_Restaurant.Pages;

public partial class Menu
{
	public const string Loading = "正在载入";
	public const string NoMoreResult = "没有更多结果";
	private string _status = string.Empty;
	public const int PageSize = 20;
	private List<Dish> _dishes = new();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
		{
			return;
		}

		_dishes.Clear();
		await LoadMoreAsync();
		StateHasChanged(); // 通知组件状态已更改
	}

	private async Task LoadMoreAsync()
	{
		try
		{
			_status = Loading;
			var dish = await _dishStorage.GetDishAsync(1);
			if (dish != null)
			{
				_dishes.Add(dish);
			}
			_status = string.Empty;
			StateHasChanged(); // 再次通知组件状态已更改
		}
		catch (Exception ex)
		{
			_status = $"加载错误: {ex.Message}";
			StateHasChanged(); // 更新状态以显示错误信息
		}
	}
}
