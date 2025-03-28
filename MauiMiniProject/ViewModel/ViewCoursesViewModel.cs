using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMiniProject.Model;
using MauiMiniProject.Services;  // เพิ่มการใช้งาน IDataService

namespace MauiMiniProject.ViewModel;

public partial class ViewCoursesViewModel : ObservableObject
{
    private readonly Iservice _dataService;  // เพิ่มการใช้งาน IDataService

    [ObservableProperty]
    ObservableCollection<Student> students = new ObservableCollection<Student>();

    // Constructor
    public ViewCoursesViewModel(Iservice dataService)
    {
        System.Diagnostics.Debug.WriteLine($"[DEBUG] SID เช็ค: {dataService.Sid}");
        _dataService = dataService;  // รับค่า IDataService
        LoadDataAsync();
    }

    // ReadJsonAsync
    async Task<List<Student>> ReadJsonAsync()
{
    try
    {
        // ดึงเส้นทางของไฟล์จาก AppDataDirectory
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "student.json");
        
        // ตรวจสอบว่าไฟล์มีอยู่หรือไม่
        if (File.Exists(filePath))
        {
            using var stream = File.OpenRead(filePath);
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            List<Student> students = Student.FromJson(contents);
            return students;
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("ไฟล์ student.json ไม่พบใน AppDataDirectory");
            return new List<Student>();
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"เกิดข้อผิดพลาด: {ex.Message}");
        return new List<Student>();
    }
}


    // LoadDataAsync
    async Task LoadDataAsync()
    {
        var jsonStudents = await ReadJsonAsync();
        
        // กรองข้อมูลที่มี sid ตรงกับค่า Sid จาก IDataService
        var filteredStudents = jsonStudents.Where(student => student.Sid == _dataService.Sid).ToList();

        // Convert to observable collection
        Students = new ObservableCollection<Student>(filteredStudents);
    }

    [RelayCommand]
    void Click(long id)
    {
        System.Diagnostics.Debug.WriteLine(id);
    }
}
