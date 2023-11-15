using Microsoft.AspNetCore.Components;
using NEU_Restaurant.Library.IServices;

namespace NEU_Restaurant.Library.Services;

public class NavigationService : INavigationService
{
	private readonly NavigationManager _navigationManager;
	private readonly ParcelBoxService _parcelBoxService;

	public NavigationService(NavigationManager navigationManager, ParcelBoxService parcelBoxService)
	{
		_navigationManager = navigationManager;
		_parcelBoxService = parcelBoxService;
	}

	public void NavigateTo(string uri) => _navigationManager.NavigateTo(uri);

	public void NavigateTo(string uri, object parameter)
	{
		var token = _parcelBoxService.Put(parameter);
		_navigationManager.NavigateTo($"{uri}/{token}");
	}
}