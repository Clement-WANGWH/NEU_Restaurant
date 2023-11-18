using Microsoft.Extensions.Logging;
using NEU_Restaurant.Data;
using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Services;
using NEU_Restaurant.Services;

namespace NEU_Restaurant
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});
			builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBootstrapBlazor();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

			builder.Services.AddSingleton<WeatherForecastService>();

			builder.Services.AddScoped<IPreferenceStorage, PreferenceStorage>();
			builder.Services.AddScoped<IFavoriteStorage, FavoriteStorage>();
			builder.Services.AddScoped<IDishStorage, DishStorage>();
			builder.Services.AddScoped<IParcelBoxService, ParcelBoxService>();
			builder.Services.AddScoped<INavigationService, NavigationService>();

			return builder.Build();
		}
	}
}