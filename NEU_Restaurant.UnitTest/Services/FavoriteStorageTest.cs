using Moq;
using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Models;
using NEU_Restaurant.Library.Services;
using NEU_Restaurant.UnitTest.Helpers;

namespace NEU_Restaurant.UnitTest.Services;

public class FavoriteStorageTest : IDisposable
{
	public FavoriteStorageTest() => FavoriteStorageHelper.RemoveDatabaseFile();

	public void Dispose() => FavoriteStorageHelper.RemoveDatabaseFile();

	[Fact]
	public async Task IsInitialized_Default()
	{
		var preferenceStorageMock = new Mock<IPreferenceStorage>();
		preferenceStorageMock
			.Setup(p => p.Get(FavoriteStorageConstant.VersionKey, default(int)))
			.Returns(FavoriteStorageConstant.Version);
		var mockPreferenceStorage = preferenceStorageMock.Object;

		var favoriteStorage = new FavoriteStorage(mockPreferenceStorage);
		Assert.True(favoriteStorage.IsInitialized);
	}

	[Fact]
	public async Task InitializeAsync_Default()
	{
		var favoriteStorage = new FavoriteStorage(GetEmptyPreferenceStorage());
		Assert.False(File.Exists(Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder
				.LocalApplicationData), FavoriteStorage.DbName)));

		await favoriteStorage.InitializeAsync();
		Assert.True(File.Exists(Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder
				.LocalApplicationData), FavoriteStorage.DbName)));

		await favoriteStorage.CloseAsync();
	}

	[Fact]
	public async Task SaveFavoriteAsync_GetFavoriteAsync_Default()
	{
		var favoriteStorage = new FavoriteStorage(GetEmptyPreferenceStorage());
		await favoriteStorage.InitializeAsync();

		var favoriteToSave = new Favorite { DishId = 1, IsFavorite = true };
		await favoriteStorage.SaveFavoriteAsync(favoriteToSave);

		var favorite =
			await favoriteStorage.GetFavoriteAsync(favoriteToSave.DishId);
		Assert.Equal(favoriteToSave.DishId, favorite.DishId);
		Assert.Equal(favoriteToSave.IsFavorite, favorite.IsFavorite);
		Assert.NotEqual(default, favorite.Timestamp);

		await favoriteStorage.SaveFavoriteAsync(favoriteToSave);
		favorite =
			await favoriteStorage.GetFavoriteAsync(favoriteToSave.DishId);
		Assert.True(DateTime.Today.Ticks < favorite.Timestamp);

		await favoriteStorage.CloseAsync();
	}

	[Fact]
	public async Task GetFavoritesAsync_Default()
	{
		var favoriteStorage = new FavoriteStorage(GetEmptyPreferenceStorage());
		await favoriteStorage.InitializeAsync();

		var favoriteListToSave = new List<Favorite>();
		var random = new Random();
		for (var i = 0; i < 5; i++)
		{
			favoriteListToSave.Add(new Favorite
			{
				DishId = i,
				IsFavorite = random.NextDouble() > 0.5
			});
			await Task.Delay(10);
		}

		var favoriteDictionary = favoriteListToSave.Where(p => p.IsFavorite)
			.ToDictionary(p => p.DishId, p => true);

		foreach (var favoriteToSave in favoriteListToSave)
		{
			await favoriteStorage.SaveFavoriteAsync(favoriteToSave);
		}

		var favoriteList = await favoriteStorage.GetFavoritesAsync();
		Assert.Equal(favoriteDictionary.Count, favoriteList.Count());
		foreach (var favorite in favoriteList)
		{
			Assert.True(favoriteDictionary.ContainsKey(favorite.DishId));
		}

		await favoriteStorage.CloseAsync();
	}

	private static IPreferenceStorage GetEmptyPreferenceStorage() =>
		new Mock<IPreferenceStorage>().Object;
}