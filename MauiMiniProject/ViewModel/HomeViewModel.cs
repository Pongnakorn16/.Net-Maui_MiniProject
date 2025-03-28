using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMiniProject.Services;

namespace MauiMiniProject.ViewModel;

public partial class HomeViewModel : ObservableObject
{
    private readonly Iservice _dataService;

    [ObservableProperty]
    string name;

    public HomeViewModel(Iservice dataService)
    {
        System.Diagnostics.Debug.WriteLine($"[DEBUG] SID เช็ค: {dataService.Sid}");
        _dataService = dataService;
        Name = _dataService.name;
        NavigateToProfileCommand = new RelayCommand(NavigateToProfile);
        NavigateToViewCoursesCommand = new RelayCommand(NavigateToViewCourses);
        NavigateToSearchCoursesCommand = new RelayCommand(NavigateToSearchCourses);
        NavigateToWithdrawCourseCommand = new RelayCommand(NavigateToWithdrawCourse);
    }

    public IRelayCommand NavigateToProfileCommand { get; }
    public IRelayCommand NavigateToViewCoursesCommand { get; }
    public IRelayCommand NavigateToSearchCoursesCommand { get; }
    public IRelayCommand NavigateToWithdrawCourseCommand { get; }

    private async void NavigateToProfile()
    {
        await Shell.Current.GoToAsync("ProfilePage");
    }

    private async void NavigateToViewCourses()
    {
        await Shell.Current.GoToAsync("ViewCoursesPage");
    }

    private async void NavigateToSearchCourses()
    {
        await Shell.Current.GoToAsync("SearchCoursesPage");
    }

    private async void NavigateToWithdrawCourse()
    {
        await Shell.Current.GoToAsync("WithdrawCoursePage");
    }
}

