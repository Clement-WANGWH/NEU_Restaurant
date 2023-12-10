using Moq;
using NEU_Restaurant.Library.IServices;
using System.Linq.Expressions;
using NEU_Restaurant.Library.Models;
using NEU_Restaurant.UnitTest.Helpers;
using DishStorageConstant = NEU_Restaurant.Library.Services.DishStorageConstant;

namespace NEU_Restaurant.UnitTest.Services;

public class DishStorageTest : IDisposable
{
	public DishStorageTest() => DishStorageHelper.RemoveDatabaseFile();

	public void Dispose() => DishStorageHelper.RemoveDatabaseFile();

	[Fact]
	public void IsInitialized_Initialized()
	{
		var preferenceStorageMock = new Mock<IPreferenceStorage>();
		preferenceStorageMock
			.Setup(p => p.Get(DishStorageConstant.DbVersionKey, 0))
			.Returns(DishStorageConstant.Version);
		var mockPreferenceStorage = preferenceStorageMock.Object;
		var DishStorage = new Library.Services.DishStorage(mockPreferenceStorage);
		Assert.True(DishStorage.IsInitialized);
	}

	[Fact]
	public void IsInitialized_NotInitialized()
	{
		var preferenceStorageMock = new Mock<IPreferenceStorage>();
		preferenceStorageMock
			.Setup(p => p.Get(DishStorageConstant.DbVersionKey, 0))
			.Returns(0);
		var mockPreferenceStorage = preferenceStorageMock.Object;
		var DishStorage = new Library.Services.DishStorage(mockPreferenceStorage);
		Assert.False(DishStorage.IsInitialized);
	}

	[Fact]
	public async Task GetDishAsync_Default()
	{
		var DishStorage = await DishStorageHelper
			.GetInitializedDishStorage();
		var Dish = await DishStorage.GetDishAsync(1);
		Assert.Equal("红烧肉", Dish.Name);
		await DishStorage.CloseAsync();
	}
}