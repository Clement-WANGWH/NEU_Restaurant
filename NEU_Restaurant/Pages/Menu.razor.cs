using System.Linq.Expressions;
using NEU_Restaurant.Library.Models;
using Microsoft.AspNetCore.Components;
using BootstrapBlazor.Components;

namespace NEU_Restaurant.Pages;

public partial class Menu
{
	public const string Loading = "正在载入";
	private string _status = string.Empty;
	private List<Item> _items = new();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
		{
			return;
		}

		_items.Clear();
		await LoadMoreAsync();
		StateHasChanged(); // 通知组件状态已更改
	}

	private async Task LoadMoreAsync()
	{
		try
		{
			_status = Loading;
            var items = await _dataIntegrationService.GetItemsAsync(
                dish => true,
                favorite => true,
                true);
			_status = string.Empty;
			_items.AddRange(items);
			StateHasChanged(); // 再次通知组件状态已更改
		}
		catch (Exception ex)
		{
			_status = $"加载错误: {ex.Message}";
            StateHasChanged(); // 更新状态以显示错误信息
        }
    }

    private Task<QueryData<Item>> OnQueryAsync(QueryPageOptions options)
    {
        var items = _items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems);
        return Task.FromResult(new QueryData<Item>()
        {
            Items = items,
            TotalCount = _items.Count
        });
    }
}
