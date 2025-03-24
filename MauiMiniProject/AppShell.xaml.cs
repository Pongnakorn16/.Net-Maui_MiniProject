using MauiMiniProject.Pages;

namespace MauiMiniProject;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
		Routing.RegisterRoute(nameof(ViewCoursesPage), typeof(ViewCoursesPage));
		Routing.RegisterRoute(nameof(SearchCoursesPage), typeof(SearchCoursesPage));
		Routing.RegisterRoute(nameof(WithdrawCoursePage), typeof(WithdrawCoursePage));
	}
}
