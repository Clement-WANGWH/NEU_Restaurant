using Microsoft.AspNetCore.Components;
using NEU_Restaurant.Library.IServices;
using NEU_Restaurant.Library.Models;

namespace NEU_Restaurant.Pages;

public partial class Detail
{
	[Inject]
	public IDishStorage DishStorage { get; set; }

	public Dish Dish { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Dish = await DishStorage.GetDishAsync(DishId);
	}
}