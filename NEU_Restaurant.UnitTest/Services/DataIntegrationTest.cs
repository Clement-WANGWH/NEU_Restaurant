using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Models;
using NEU_Restaurant.UnitTest.Helpers;

namespace NEU_Restaurant.UnitTest.Services;

public class DataIntegrationTest : IDisposable
{
    public void Dispose() => DishStorageHelper.RemoveDatabaseFile();

    public IDataIntegrationService _dataIntegrationService;

    private List<Item> _items = new();

    [Fact]
    public async Task GetItemsAsync_Default()
    {
        var items = await _dataIntegrationService.GetItemsAsync(dish => true, favorite => true, false);

        Assert.Equal(3, items.Count());
    }


}
