namespace NEU_Restaurant.Library.IServices;

public interface INavigationService
{
	void NavigateTo(string uri);

}

public static class NavigationServiceConstants
{
	public const string MenuPage = "/Menu";
	public const string InitializationPage = "/initialization";
}