using Moq;
using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Services;

namespace NEU_Restaurant.UnitTest.Helpers;

public class DishStorageHelper
{
	public static void RemoveDatabaseFile() =>
		File.Delete(DishStorage.DishDbPath);

	public static async Task<DishStorage> GetInitializedDishStorage()
	{
		var preferenceStorageMock = new Mock<IPreferenceStorage>();
		preferenceStorageMock.Setup(p =>
			p.Get(DishStorageConstant.DbVersionKey, -1)).Returns(-1);
		var mockPreferenceStorage = preferenceStorageMock.Object;
		var DishStorage = new DishStorage(mockPreferenceStorage);
		await DishStorage.InitializeAsync();
		return DishStorage;
	}
}