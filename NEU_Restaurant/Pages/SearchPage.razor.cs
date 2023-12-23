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

    private IEnumerable<string> Autofill = new List<string>();

    protected override async Task OnInitializedAsync()
    {
        Autofill = await _dataIntegrationService.GetNameAndFlavorAsync(dish => true);
        _beenSearched = false;
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
        _ = _items.Count == 0 ? _status = NoData : _status = NoMoreData;
        StateHasChanged();
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