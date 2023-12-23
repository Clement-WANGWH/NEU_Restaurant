using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using NEU_Restaurant.Library.Models;
using Microsoft.AspNetCore.Components;
using BootstrapBlazor.Components;
using Microsoft.Extensions.Logging;
using Item = NEU_Restaurant.Library.Models.Item;

namespace NEU_Restaurant.Pages;

public partial class Menu
{
	public const string Loading = "正在载入";
    public const string NoMoreData = "页面到底了";
    public const string NoData = "没有相关数据";

    private string _status = string.Empty;
    private string _canteen = string.Empty;
    private string _stall = string.Empty;
    private string _flavor = string.Empty;
    private bool _rank = true;
    private bool _israted = false;
    private List<Item> _items = new();
    private static readonly List<SelectedItem> value = new();

    private List<SelectedItem> _canteens { get; set; } = value;
    private IEnumerable<SelectedItem>? _stalls { get; set; }
    private List<SelectedItem> _flavors { get; set; } = new();
    private Dictionary<string, IEnumerable<string>> _canteenStallMap { get; set; } = new();
    [Range(0, 25)]
    public int HighValue { get; set; } = 20;
    [Range(0, 25)]
    public int LowValue { get; set; } = 0;

    private IEnumerable<int> _rate { get; set; } = Enumerable.Range(1, 6);
    private IEnumerable<SelectedItem>? Points { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _canteens = (await _dataIntegrationService.GetCanteensAsync(dish => true)).Select(canteen => new SelectedItem(canteen, canteen)).ToList();
        _flavors = (await _dataIntegrationService.GetFlavorsAsync(dish => true)).Select(flavor => new SelectedItem(flavor, flavor)).ToList();
        _canteenStallMap = await _dataIntegrationService.GetCanteenStallMapAsync(dish => true);
        _stalls = _canteenStallMap[_canteens[0].Value].Select(stall => new SelectedItem(stall, stall));
        _flavor = _flavors[0].Value;
        _stall = _stalls.First().Value;
        _canteen = _canteens[0].Value;
        Points = new List<SelectedItem>(new List<SelectedItem>
        {
            new SelectedItem { Text = "1分", Value = "1" },
            new SelectedItem { Text = "2分", Value = "2" },
            new SelectedItem { Text = "3分", Value = "3" },
            new SelectedItem { Text = "4分", Value = "4" },
            new SelectedItem { Text = "5分", Value = "5" },
        });
    }

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
            _ = !items.Any() ? _status = NoData : _status = NoMoreData;
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

    private async Task OnCascadeBindSelectClick(SelectedItem item)
    {
        await Task.Delay(3);
        _canteen = item.Value;
        _stalls = _canteenStallMap[item.Value].Select(stall => 
            new SelectedItem(stall, stall)) ?? Enumerable.Empty<SelectedItem>();
        StateHasChanged();
    }

    private Task StallValueChanged(SelectedItem item)
    {
        _stall = item.Value;
        return Task.CompletedTask;
    }

    private Task FlavorValueChanged(SelectedItem item)
    {
        _flavor = item.Value;
        return Task.CompletedTask;
    }

    private async Task ClickAsyncButton()
    {
        if (_israted)
        {
            _items = (await _dataIntegrationService.GetRatedItemsAsync(dish =>
                                   (_canteen.Equals("全部食堂") || _canteen.Equals(dish.Canteen)) &&
                                                      (_stall.Equals("全部档口") || _stall.Equals(dish.Stall)) &&
                                                      (_flavor.Equals(dish.Flavor) || _flavor.Equals("全部口味")) &&
                                                      (HighValue >= dish.Price) && (LowValue <= dish.Price),
                               favorite => (_rate.Contains(favorite.DishRate)),
                                              _rank)).ToList();
        }else
        {
            _items = (await _dataIntegrationService.GetItemsAsync(dish =>
                    (_canteen.Equals("全部食堂") || _canteen.Equals(dish.Canteen)) &&
                    (_stall.Equals("全部档口") || _stall.Equals(dish.Stall)) &&
                    (_flavor.Equals(dish.Flavor) || _flavor.Equals("全部口味")) &&
                    (HighValue >= dish.Price) && (LowValue <= dish.Price),
                favorite => (_rate.Contains(favorite.DishRate)),
                _rank)).ToList();
        }
        _ = _items.Count == 0 ? _status = NoData : _status = NoMoreData;
        StateHasChanged();
    }

    private async Task OnListViewItemClick(Item item)
    {
        var detail = new DialogOption
        {
            Title = "菜品详情",
            ShowFooter = false,
            BodyContext = item.Id,
            Component = BootstrapDynamicComponent.CreateComponent<Detail>()
        };
        await DialogService.Show(detail);
    }
}
