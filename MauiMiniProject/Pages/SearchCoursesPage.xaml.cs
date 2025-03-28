using MauiMiniProject.Services;
using MauiMiniProject.ViewModel;

namespace MauiMiniProject.Pages;

public partial class SearchCoursesPage : ContentPage
{
    public SearchCoursesPage()
{
    InitializeComponent();
    BindingContext = new SearchViewModel(DependencyService.Get<Iservice>()); // ใช้ DataService ที่ถูก inject เข้ามา
}

}