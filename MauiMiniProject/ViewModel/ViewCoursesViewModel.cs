using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMiniProject.Model;
using MauiMiniProject.Services;  // เพิ่มการใช้งาน IDataService

namespace MauiMiniProject.ViewModel;

public partial class ViewCoursesViewModel : ObservableObject
{
    private readonly Iservice _dataService;  // เพิ่มการใช้งาน IDataService
    public ICommand BackCommand { get; }

    [ObservableProperty]
    ObservableCollection<Student> students = new ObservableCollection<Student>();

    // Constructor
    public ViewCoursesViewModel(Iservice dataService)
    {
        BackCommand = new Command(Back);
        _dataService = dataService;  // รับค่า IDataService
        LoadDataAsync();
    }

    // ReadJsonAsync
    private async Task<List<Student>> ReadJsonAsync()
{
    try
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "student.json");

        // ตรวจสอบไฟล์ที่มีอยู่ในแอป
        if (File.Exists(filePath))
        {
            // เปิดไฟล์เป็น Stream แล้วส่งไปยังฟังก์ชัน ReadFileAsync
            using var stream = File.OpenRead(filePath);
            return await ReadFileAsync(stream);
        }
        else
        {
            // โหลดไฟล์จาก package
            var packageFile = await FileSystem.OpenAppPackageFileAsync("student.json");
            return await ReadFileAsync(packageFile);
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"เกิดข้อผิดพลาด: {ex.Message}");
        return new List<Student>(); // กรณีเกิดข้อผิดพลาด ให้คืนค่าเป็น List ว่าง
    }
}


    // Helper function to read file content
    private async Task<List<Student>> ReadFileAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        var contents = await reader.ReadToEndAsync();
        return Student.FromJson(contents);
    }
    		public async void Back()
{
    await Shell.Current.GoToAsync("HomePage");
    
}

    // LoadDataAsync
    private async Task LoadDataAsync()
    {
        var jsonStudents = await ReadJsonAsync();

        // ตรวจสอบว่า Sid มีค่าหรือไม่
        if (_dataService.Sid == 0)
        {
            System.Diagnostics.Debug.WriteLine("Sid ไม่มีค่าใน IDataService");
            return;
        }

        // กรองข้อมูลที่มี Sid ตรงกับค่า Sid จาก IDataService
        var filteredStudents = jsonStudents.Where(student => student.Sid == _dataService.Sid).ToList();

        // อัปเดต ObservableCollection ด้วยข้อมูลที่กรองแล้ว
        Students.Clear();
        foreach (var student in filteredStudents)
        {
            Students.Add(student);
        }
    }

    [RelayCommand]
    void Click(long id)
    {
        System.Diagnostics.Debug.WriteLine($"Clicked on student with ID: {id}");
    }
}
