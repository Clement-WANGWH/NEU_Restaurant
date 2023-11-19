using NEU_Restaurant.Library.Services;


namespace NEU_Restaurant.UnitTest.Helpers;
public class FavoriteStorageHelper
{
	public static void RemoveDatabaseFile() =>
		File.Delete(FavoriteStorage.DishDbPath);
}
