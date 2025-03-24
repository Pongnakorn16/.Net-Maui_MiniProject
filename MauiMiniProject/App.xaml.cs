using MauiMiniProject.Services;

namespace MauiMiniProject;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		DependencyService.Register<Iservice, DataService>();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}