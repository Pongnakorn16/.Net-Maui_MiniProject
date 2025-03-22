using MauiMiniProject.ViewModel;

namespace MauiMiniProject.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel();
	}

	private async void ForgetPasswordTapped(object sender, EventArgs e)
	{
    //     var loginData = new LoginData {
    //         Uname = uname.Text,
    //         Password = pwd.Text
    //     };
    //     var queryParams = new Dictionary<String, object>{
    //         {"data",loginData}
    //     };

    //     System.Diagnostics.Debug.WriteLine($"forgetpassword?uname={uname.Text}&pwd={pwd.Text}");
    //     System.Diagnostics.Debug.WriteLine($"{nameof(ForgetPasswordPage)}?uname={uname.Text}&pwd={pwd.Text}");
    //     // await Shell.Current.GoToAsync($"forgetpassword?uname={uname.Text}&pwd={pwd.Text}");
    //     await Shell.Current.GoToAsync("forgetpassword", queryParams);

    //     //แบบ nameof ไม่รู้ทำไมใช้ไม่ได้
    //     // await Shell.Current.GoToAsync($"{nameof(ForgetPasswordPage)}?uname={uname.Text}&pwd={pwd.Text}");
    //     // await Shell.Current.GoToAsync($"{nameof(ForgetPasswordPage)}{queryParams}");
	}
}

 public class LoginData {
    public String Email {get; set;} = "";
    public String Password {get; set;} = "";
}
