using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Models;
using SQLite;

namespace NEU_Restaurant.Library.Services;

public class FavoriteStorage : IFavoriteStorage
{
	public const string DbName = "favoritedb.sqlite3";

	public static readonly string DishDbPath =
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);

	private SQLiteAsyncConnection? _connection;

	private SQLiteAsyncConnection Connection => _connection ??= new SQLiteAsyncConnection(DishDbPath);

	private readonly IPreferenceStorage _preferenceStorage;

	public FavoriteStorage(IPreferenceStorage preferenceStorage)
	{
		_preferenceStorage = preferenceStorage;
	}

	public bool IsInitialized =>
		_preferenceStorage.Get(FavoriteStorageConstant.VersionKey, default(int)) == FavoriteStorageConstant.Version;

	public async Task InitializeAsync()
	{
		await Connection.CreateTableAsync<Favorite>(); 
		_preferenceStorage.Set(FavoriteStorageConstant.VersionKey, FavoriteStorageConstant.Version);
	}

	public async Task<Favorite?> GetFavoriteAsync(int DishId) =>
		await Connection.Table<Favorite>()
			.FirstOrDefaultAsync(p => p.DishId == DishId);

	public async Task<IEnumerable<Favorite>> GetFavoritesAsync() =>
		await Connection.Table<Favorite>().Where(p => p.IsFavorite)
			.OrderByDescending(p => p.Timestamp).ToListAsync();

	public async Task<IEnumerable<Favorite>> GetDislikedAsync() =>
		await Connection.Table<Favorite>().Where(p => p.IsDisliked)
			.OrderByDescending(p => p.Timestamp).ToListAsync();
	
	public async Task SaveFavoriteAsync(Favorite favorite)
	{
		favorite.Timestamp = DateTime.Now.Ticks;
		await Connection.InsertOrReplaceAsync(favorite);
	}

	public async Task CloseAsync() => await Connection.CloseAsync();
}

public static class FavoriteStorageConstant
{
	public const string VersionKey =
		nameof(FavoriteStorageConstant) + "." + nameof(Version);

	public const int Version = 1;
}