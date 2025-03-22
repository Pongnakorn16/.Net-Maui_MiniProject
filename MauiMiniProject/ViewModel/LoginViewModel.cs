using System;
using CommunityToolkit.Mvvm.ComponentModel;
 using CommunityToolkit.Mvvm.Input;
using MauiMiniProject.Pages;


namespace MauiMiniProject.ViewModel;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    string email = "";

    [ObservableProperty]
    
    string password = "";

    [ObservableProperty]
    string errorMessage = "";

    [ObservableProperty]
    bool isErrorVisible = false;

    [ObservableProperty]
    string register = nameof(RegisterPage);
    
    [RelayCommand]
    async Task Login()
    {
        if (Email == "Dok@dok" && Password == "123")
        {
            
            await GoToPage(nameof(HomePage));
        }
        else if (Email != "Dok@dok" && Password == "123")
        {
            
            ErrorMessage = "Email ไม่ถูกต้อง";
            IsErrorVisible = true;
        }
        else if (Email == "Dok@dok" && Password != "123")
        {
            
            ErrorMessage = "Password ไม่ถูกต้อง";
            IsErrorVisible = true;
        }
        else if (Email != "Dok@dok" && Password != "123" && Email != "" && Password != "")
        {
            
            ErrorMessage = "Email และ Password ไม่ถูกต้อง";
            IsErrorVisible = true;
        }
        else if (Email == "" && Password != "")
        {
            
            ErrorMessage = "โปรดใส่ Email";
            IsErrorVisible = true;
        }
        else if (Email != "" && Password == "")
        {
            
            ErrorMessage = "โปรดใส่ Password";
            IsErrorVisible = true;
        }
        else if (Email == "" && Password == "")
        {
            
            ErrorMessage = "โปรดใส่ข้อมูลก่อน";
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    async Task GoToPage(string page){
      await Shell.Current.GoToAsync(page);
    }
}
