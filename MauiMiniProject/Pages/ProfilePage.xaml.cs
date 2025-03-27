using MauiMiniProject.Services;
using MauiMiniProject.ViewModel;

namespace MauiMiniProject.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = new ProfileViewModel(DependencyService.Get<Iservice>());
    }
}
