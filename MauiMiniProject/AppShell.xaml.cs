using MauiMiniProject.Pages;

namespace MauiMiniProject;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
		Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
		Routing.RegisterRoute(nameof(RegisteredCoursesPage), typeof(RegisteredCoursesPage));
		Routing.RegisterRoute(nameof(SearchCoursesPage), typeof(SearchCoursesPage));
		Routing.RegisterRoute(nameof(WithdrawCoursePage), typeof(WithdrawCoursePage));
	}
}
