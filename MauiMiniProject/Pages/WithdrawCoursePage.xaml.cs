using MauiMiniProject.Services;
using MauiMiniProject.ViewModel;

namespace MauiMiniProject.Pages;

public partial class WithdrawCoursePage : ContentPage
{
	public WithdrawCoursePage()
	{
		InitializeComponent();
		BindingContext = new WithdrawViewModel(DependencyService.Get<Iservice>());
	}
}