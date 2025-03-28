using MauiMiniProject.Services;
using MauiMiniProject.ViewModel;

namespace MauiMiniProject.Pages;

public partial class ViewCoursesPage : ContentPage
{
	public ViewCoursesPage()
{
    InitializeComponent();
    BindingContext = new ViewCoursesViewModel(DependencyService.Get<Iservice>());
}

}