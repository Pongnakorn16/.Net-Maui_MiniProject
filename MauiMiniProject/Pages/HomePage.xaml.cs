using MauiMiniProject.Pages;
using MauiMiniProject.Services;
using MauiMiniProject.ViewModel;

namespace MauiMiniProject.Pages
{
    public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        var dataService = DependencyService.Get<Iservice>();
        BindingContext = new HomeViewModel(dataService);
    }
}

}
