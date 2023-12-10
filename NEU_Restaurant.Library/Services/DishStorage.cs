using System.Linq.Expressions;
using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Models;
using SQLite;

namespace NEU_Restaurant.Library.Services;

public class DishStorage : IDishStorage
{
	public const string DbName = "dishdb.sqlite3";

	public static readonly string DishDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);

	private SQLiteAsyncConnection _connection;

	private SQLiteAsyncConnection Connection =>_connection ??= new SQLiteAsyncConnection(DishDbPath);

	private readonly IPreferenceStorage _preferenceStorage;

	public DishStorage(IPreferenceStorage preferenceStorage)
	{
		_preferenceStorage = preferenceStorage;
	}

	public bool IsInitialized =>
		_preferenceStorage.Get(DishStorageConstant.DbVersionKey, 0) == DishStorageConstant.Version;

	public async Task InitializeAsync()
	{
		
		await using var dbFileStream = new FileStream(DishDbPath, FileMode.OpenOrCreate);
		await using var dbAssetStream = typeof(DishStorage).Assembly.GetManifestResourceStream(DbName);
		await dbAssetStream.CopyToAsync(dbFileStream);
		_preferenceStorage.Set(DishStorageConstant.DbVersionKey, DishStorageConstant.Version);
		
	}

	public Task<Dish> GetDishAsync(int id) => Connection.Table<Dish>().FirstOrDefaultAsync(p => p.Id == id);

	public async Task<IEnumerable<Dish>> GetDishesAsync(
		Expression<Func<Dish, bool>> where) =>
		await Connection.Table<Dish>().Where(where).ToListAsync();

	public async Task CloseAsync() => await Connection.CloseAsync();
}

public static class DishStorageConstant
{
	public const string DbVersionKey = nameof(DishStorageConstant) + "." + nameof(DbVersionKey);
	public const int Version = 4;
}