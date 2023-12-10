using Microsoft.AspNetCore.Components;
using NEU_Restaurant.Library.IServices;

namespace NEU_Restaurant.Library.Services;

public class NavigationService : INavigationService
{
	private readonly NavigationManager _navigationManager;

	public NavigationService(NavigationManager navigationManager)
	{
		_navigationManager = navigationManager;
	}

	public void NavigateTo(string uri) => _navigationManager.NavigateTo(uri);

}