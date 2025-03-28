using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiMiniProject.Model;
using MauiMiniProject.Services;

public partial class ProfileViewModel : ObservableObject
{
    private readonly Iservice _dataService;

    [ObservableProperty]
    ObservableCollection<Student> studentdata = new ObservableCollection<Student>();

    public ICommand LogoutCommand { get; }

    public ICommand BackCommand { get; }
    // Constructor
    public ProfileViewModel(Iservice dataService)
    {
        BackCommand = new Command(Back);
        LogoutCommand = new Command(Logout);
        System.Diagnostics.Debug.WriteLine($"[DEBUG] SID เช็ค: {dataService.Sid}");
        _dataService = dataService;
        LoadDataStudent();
    }

    async Task<List<Student>> ReadJsonAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("student.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            List<Student> students = Student.FromJson(contents);
            return students;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return new List<Student>();
        }
    }

    // Load Data
    async Task LoadDataStudent()
    {
        var jsonStudents = await ReadJsonAsync();
        var filteredStudents = jsonStudents.Where(student => student.Sid == _dataService.Sid).ToList();
        Studentdata = new ObservableCollection<Student>(filteredStudents);
    }

        public async void Logout()
{
    ClearStudentJsonOnLogout();
    await Shell.Current.GoToAsync("LoginPage");
    
}

 public async void Back()
{
    await Shell.Current.GoToAsync("HomePage");
    
}


    public void ClearStudentJsonOnLogout()
{
    string filePath = Path.Combine(FileSystem.AppDataDirectory, "student.json");

    if (File.Exists(filePath))
    {
        File.Delete(filePath);
    }
}
}
