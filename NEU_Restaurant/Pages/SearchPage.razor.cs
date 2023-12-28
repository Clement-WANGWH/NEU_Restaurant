using BootstrapBlazor.Components;
using Microsoft.Extensions.Logging;
using NEU_Restaurant.Library.Models;

namespace NEU_Restaurant.Pages;

public partial class SearchPage
{
    public const string Loading = "正在载入";
    public const string NoMoreData = "页面到底了";
    public const string NoData = "没有相关数据";

    private string _status = string.Empty;
    private bool _beenSearched;

    private List<Item> _items = new();
    private List<Item> _showItems = new();

    private int PageItems { get; set; } = 5;
    private int PageCount => _items == null
        ? 0
        : _items.Count % PageItems == 0
            ? _items.Count / PageItems
            : _items.Count / PageItems + 1;
    private string PageInfoText => $"每页 {PageItems} 条 共 {PageCount} 页";
    private List<SelectedItem>? PageItemsSource { get; set; }

    private IEnumerable<string> Autofill = new List<string>();

    protected override async Task OnInitializedAsync()
    {
        Autofill = await _dataIntegrationService.GetNameAndFlavorAsync(dish => true);
        _beenSearched = false;
        PageItemsSource = new List<SelectedItem>(new List<SelectedItem>
        {
            new SelectedItem { Text = "5条", Value = "5" },
            new SelectedItem { Text = "10条", Value = "10" },
            new SelectedItem { Text = "25条", Value = "25" },
        });
    }

    private async Task OnSearch(string searchText)
    {
        _beenSearched = true;
        _items = (await _dataIntegrationService.GetItemsAsync(
                    dish =>
                        dish.Name.Contains(searchText) ||
                        dish.Canteen.Contains(searchText) ||
                        dish.Stall.Contains(searchText) ||
                        dish.Flavor.Contains(searchText) ||
                        dish.Ingredient.Contains(searchText),
                    fav => true, true)).ToList();
        await ShowItemsAsync(1);
    }

    private Task ShowItemsAsync(int pageIndex)
    {
        try
        {
            _status = Loading;
            _showItems.Clear();
            _showItems.AddRange(pageIndex == 1
                ? _items.Take(PageItems)
                : _items.Skip((pageIndex - 1) * PageItems).Take(PageItems));
            _ = !_showItems.Any() ? _status = NoData : _status = NoMoreData;
            StateHasChanged(); // 再次通知组件状态已更改
        }
        catch (Exception ex)
        {
            _status = $"加载错误: {ex.Message}";
            StateHasChanged(); // 更新状态以显示错误信息
        }
        return Task.CompletedTask;
    }

    private async Task OnListViewItemClick(Item item)
    {
        var detail = new DialogOption
        {
            Title = item.Name,
            ShowFooter = false,
            BodyContext = item.Id,
            Component = BootstrapDynamicComponent.CreateComponent<Detail>()
        };
        await DialogService.Show(detail);
    }
}