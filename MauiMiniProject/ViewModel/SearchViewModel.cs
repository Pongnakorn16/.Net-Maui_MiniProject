using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMiniProject.Model;
using Newtonsoft.Json;
using System.IO;
using MauiMiniProject.Services;
using System.Windows.Input;

namespace MauiMiniProject.ViewModel
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly Iservice _dataService;

        [ObservableProperty]
        string c_name = "";

        [ObservableProperty]
        string errorMessage = "";

        [ObservableProperty]
        bool isErrorVisible = false;

        public ICommand BackCommand { get; }

        [ObservableProperty]
        ObservableCollection<Courses> searchcourse = new ObservableCollection<Courses>();

        [ObservableProperty]
        ObservableCollection<Courses> allCourses = new ObservableCollection<Courses>();

        public SearchViewModel() { } // Constructor ว่างเพื่อให้ XAML ใช้งานได้

        public SearchViewModel(Iservice dataService)
        {
            BackCommand = new Command(Back);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] SID เช็ค: {dataService.Sid}");
            _dataService = dataService;
            LoadDataAsync();
        }

        // 📂 ฟังก์ชันตรวจสอบและคัดลอก student.json ไปที่ AppDataDirectory
        async Task<string> GetStudentJsonPath()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, "student.json");

            if (!File.Exists(path))
            {
                try
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync("student.json");
                    using var reader = new StreamReader(stream);
                    string content = await reader.ReadToEndAsync();
                    File.WriteAllText(path, content);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ERROR] คัดลอก JSON ไม่สำเร็จ: {ex.Message}");
                }
            }

            return path;
        }

        // 📖 อ่านข้อมูล student.json จาก AppDataDirectory
        async Task<List<Student>> ReadStudentJsonAsync()
        {
            try
            {
                var path = Path.Combine(FileSystem.AppDataDirectory, "student.json");

                if (!File.Exists(path))
                {
                    System.Diagnostics.Debug.WriteLine("[WARNING] ไม่พบ student.json ใน AppDataDirectory, โหลดจาก AppPackage แทน");
                    using var stream = await FileSystem.OpenAppPackageFileAsync("student.json");
                    using var reader = new StreamReader(stream);
                    var contents = await reader.ReadToEndAsync();
                    return Student.FromJson(contents);
                }

                var json = await File.ReadAllTextAsync(path);
                var students = Student.FromJson(json);
                System.Diagnostics.Debug.WriteLine($"[JSON] โหลดข้อมูลนักศึกษา: {students.Count} คน");
                return students;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] โหลด JSON ไม่สำเร็จ: {ex.Message}");
                return new List<Student>();
            }
        }

        // 💾 บันทึกข้อมูล student.json ไปที่ AppDataDirectory
        async Task SaveStudentJsonAsync(List<Student> students)
        {
            try
            {
                string path = await GetStudentJsonPath();
                var json = JsonConvert.SerializeObject(students, Formatting.Indented);
                await File.WriteAllTextAsync(path, json);
                System.Diagnostics.Debug.WriteLine("[JSON] บันทึกข้อมูลนักศึกษาสำเร็จ");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] บันทึก JSON ไม่สำเร็จ: {ex.Message}");
            }
        }

        // 📖 อ่านข้อมูล courses.json
        async Task<List<Courses>> ReadJsonAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("courses.json");
                using var reader = new StreamReader(stream);
                var contents = await reader.ReadToEndAsync();
                List<Courses> course = Courses.FromJson(contents);
                System.Diagnostics.Debug.WriteLine($"[JSON] โหลดข้อมูลสำเร็จ: {course.Count} รายวิชา");
                return course;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] โหลด JSON ไม่สำเร็จ: {ex.Message}");
                return new List<Courses>();
            }
        }

        // 📦 โหลดข้อมูลวิชาทั้งหมด
        async Task LoadDataAsync()
        {
            var jsonCourses = await ReadJsonAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                allCourses.Clear();
                foreach (var course in jsonCourses)
                {
                    allCourses.Add(course);
                }

                searchcourse.Clear();
                foreach (var course in allCourses)
                {
                    searchcourse.Add(course);
                }
            });
        }

        // 🔍 ค้นหารายวิชา
        [RelayCommand]
        async Task Search()
        {
            if (string.IsNullOrEmpty(C_name))
            {
                searchcourse.Clear();
                foreach (var course in allCourses)
                {
                    searchcourse.Add(course);
                }
                return;
            }

            var filteredCourses = allCourses
                .Where(c => c.Name.Trim().ToLower().StartsWith(C_name.Trim().ToLower()))
                .ToList();

            if (filteredCourses.Any())
            {
                searchcourse.Clear();
                foreach (var course in filteredCourses)
                {
                    searchcourse.Add(course);
                }
            }
            else
            {
                ErrorMessage = "ไม่พบรายวิชาที่ค้นหา";
                IsErrorVisible = true;
            }
        }

        public async void Back()
        {
            await Shell.Current.GoToAsync("HomePage");

        }

        // 📝 คำสั่งลงทะเบียนวิชา
        [RelayCommand]
        async Task Register(Courses course)
        {
            System.Diagnostics.Debug.WriteLine("เริ่มลงทะเบียน");
            System.Diagnostics.Debug.WriteLine($"SID ที่ใช้ลงทะเบียน: {_dataService.Sid}");

            var sid = _dataService.Sid;
            System.Diagnostics.Debug.WriteLine($"[DEBUG] SID ที่ใช้ลงทะเบียน: {sid}");
            var students = await ReadStudentJsonAsync();

            System.Diagnostics.Debug.WriteLine($"[JSON] โหลดข้อมูลนักศึกษา: {students.Count} คน");

            // ตรวจสอบว่า sid มีอยู่ใน student.json ไหม
            foreach (var studentItem in students)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] ตรวจสอบ student: Sid={studentItem.Sid}");
            }

            var foundStudent = students.FirstOrDefault(s => s.Sid == sid);
            if (foundStudent == null)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] ไม่พบข้อมูลนักศึกษาที่มี SID: {sid}");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"[SUCCESS] พบข้อมูลนักศึกษา SID: {sid}");

            // ตรวจสอบข้อมูลภายในปีการศึกษาของนักศึกษา
            var yearData = foundStudent.Year.FirstOrDefault();
            if (yearData != null)
            {
                var term3Courses = yearData.CoursesYear.FirstOrDefault(c => c.RegisteredTerm3 != null);
                if (term3Courses != null)
                {
                    // ตรวจสอบว่าได้ลงทะเบียนวิชานี้แล้วหรือยัง
                    var courseExists = term3Courses.RegisteredTerm3.Any(c => c.Cid == course.Cid.ToString());
                    if (!courseExists)
                    {
                        // เพิ่มวิชาในเทอม 3
                        term3Courses.RegisteredTerm3.Add(new RegisteredTerm { Cid = course.Cid.ToString() });
                        term3Courses.RegisteredTerm3.Add(new RegisteredTerm { Cid = course.Cid.ToString(), C_name = course.Name });

                        System.Diagnostics.Debug.WriteLine($"เพิ่มวิชา {course.Name} ลงในเทอม 3");

                        try
                        {
                            // บันทึกข้อมูลนักศึกษา
                            await SaveStudentJsonAsync(students);
                            System.Diagnostics.Debug.WriteLine("บันทึกสำเร็จ");

                            // แสดงหน้าต่างแจ้งเตือนเมื่อบันทึกสำเร็จ
                            await MainThread.InvokeOnMainThreadAsync(async () =>
{
    await Application.Current.MainPage.DisplayAlert("สำเร็จ", "บันทึกข้อมูลนักศึกษาสำเร็จ", "ตกลง");
});
                        }
                        catch (Exception ex)
                        {
                            // ถ้ามีข้อผิดพลาดในการบันทึก
                            System.Diagnostics.Debug.WriteLine($"เกิดข้อผิดพลาด: {ex.Message}");
                            await Application.Current.MainPage.DisplayAlert("ข้อผิดพลาด", "เกิดข้อผิดพลาดในการบันทึกข้อมูล", "ตกลง");
                        }
                    }
                    else
                    {
                        // ถ้าวิชานี้ลงทะเบียนไปแล้ว
                        await MainThread.InvokeOnMainThreadAsync(async () =>
{
    await Application.Current.MainPage.DisplayAlert("ไม่สำเร็จ", "ได้ทำการลงทะเบียนไปแล้วในเทอมปัจจุบัน", "ตกลง");
                        });
                        System.Diagnostics.Debug.WriteLine($"วิชา {course.Name} ได้ทำการลงทะเบียนไปแล้วในเทอม 3");
                        
                        ErrorMessage = $"วิชา {course.Name} ได้ทำการลงทะเบียนไปแล้วในเทอม 3";
                        IsErrorVisible = true;
                    }
                }
                else
                {
                    // ถ้าไม่พบข้อมูลเทอม 3
                    System.Diagnostics.Debug.WriteLine("[ERROR] ไม่พบข้อมูลเทอม 3 สำหรับนักศึกษานี้");
                    ErrorMessage = "ไม่พบข้อมูลเทอม 3 สำหรับนักศึกษานี้";
                    IsErrorVisible = true;
                }
            }
            else
            {
                // ถ้าไม่พบข้อมูลปีการศึกษาของนักศึกษา
                System.Diagnostics.Debug.WriteLine("[ERROR] ไม่พบข้อมูลปีการศึกษา");
                ErrorMessage = "ไม่พบข้อมูลปีการศึกษาของนักศึกษานี้";
                IsErrorVisible = true;
            }
        }
    }
}
