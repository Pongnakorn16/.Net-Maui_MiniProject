using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMiniProject.Model;
using MauiMiniProject.Pages;
using MauiMiniProject.Services;


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

    // [ObservableProperty]
    // string register = nameof(RegisterPage);


    [ObservableProperty]
    ObservableCollection<Student> students = new ObservableCollection<Student>();

    async Task<List<Student>> ReadJsonAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("student.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            List<Student> student = Student.FromJson(contents);
            return student;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return new List<Student>();
        }
    }

    public LoginViewModel()
    {
        LoadDataAsync();
    }

    // LoadDataAsync
    async Task LoadDataAsync()
    {
        var jsonUsers = await ReadJsonAsync();
        // Convert to observable collection
        Students = new ObservableCollection<Student>(jsonUsers);
    }

    [RelayCommand]
    async Task Login()
    {
        if (Students == null || Students.Count == 0)
        {
            ErrorMessage = "ไม่มีข้อมูลนักเรียน";
            IsErrorVisible = true;
            return;
        }


        var user = Students.FirstOrDefault(s => s.Email == Email && s.Password == Password);

        if (user != null)
        {
            var dataService = DependencyService.Get<Iservice>();
            dataService.name = user.Name;
            dataService.Sid = user.Sid;
            System.Diagnostics.Debug.WriteLine($"[DEBUG] SID เช็ค: {dataService.Sid}");
            ClearStudentJsonOnLogout();
            await Shell.Current.GoToAsync(nameof(HomePage));
        }
        else
        {

            ErrorMessage = "Email หรือ Password ไม่ถูกต้อง";
            IsErrorVisible = true;
        }
    }

    public void ClearStudentJsonOnLogout()
{
    string filePath = Path.Combine(FileSystem.AppDataDirectory, "student.json");

    if (File.Exists(filePath))
    {
        File.Delete(filePath);
    }
}


    [RelayCommand]
    async Task GoToPage(string page)
    {
        await Shell.Current.GoToAsync(page);
    }
}
