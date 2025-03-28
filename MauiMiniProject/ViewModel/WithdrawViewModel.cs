using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMiniProject.Model;
using MauiMiniProject.Services;
using Newtonsoft.Json;

namespace MauiMiniProject.ViewModel
{
    public partial class WithdrawViewModel : ObservableObject
    {
        private readonly Iservice _dataService;  // เพิ่มการใช้งาน IDataService

        public ICommand WithdrawCommand { get; }
		public ICommand BackCommand { get; }

        [ObservableProperty]
        ObservableCollection<Student> students = new ObservableCollection<Student>();

        // สร้าง Dictionary สำหรับเก็บชื่อวิชาตาม SID
        private Dictionary<long, List<string>> studentCourseNames = new Dictionary<long, List<string>>();

        // Constructor
        public WithdrawViewModel(Iservice dataService)
        {
			BackCommand = new Command(Back);
            WithdrawCommand = new Command<int>(async (courseId) => await Withdraw(courseId));

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

                // ตรวจสอบว่าไฟล์ใน AppDataDirectory มีอยู่หรือไม่
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
                    // หากไม่มีใน AppDataDirectory ให้ดึงจากแอปแพ็คเกจ
                    var packageFile = await FileSystem.OpenAppPackageFileAsync("student.json");
                    using var reader = new StreamReader(packageFile);
                    var contents = await reader.ReadToEndAsync();
                    List<Student> students = Student.FromJson(contents);

                    // ดึงข้อมูลจาก courses.json โดยใช้ cid จาก Student
                    var coursesFile = await FileSystem.OpenAppPackageFileAsync("courses.json");
                    using var coursesReader = new StreamReader(coursesFile);
                    var coursesContents = await coursesReader.ReadToEndAsync();
                    List<Courses> courses = Courses.FromJson(coursesContents);

                    // นำ cid ที่ได้จาก student มาค้นหาข้อมูลใน courses
                    foreach (var student in students)
                    {
                        List<string> studentCourses = new List<string>();  // สร้าง List สำหรับเก็บชื่อวิชาของ student

                        foreach (var year in student.Year)
                        {
                            foreach (var coursesYear in year.CoursesYear)
                            {
                                var registeredTerms = new List<RegisteredTerm>();
                                registeredTerms.AddRange(coursesYear.RegisteredTerm3);
                                registeredTerms.AddRange(coursesYear.RegisteredTerm2);
                                registeredTerms.AddRange(coursesYear.RegisteredTerm1);

                                foreach (var registeredTerm in registeredTerms)
                                {
                                    var course = courses.FirstOrDefault(c => c.Cid.ToString() == registeredTerm.Cid);
                                    if (course != null)
                                    {
                                        // เก็บชื่อวิชาลงใน studentCourses
                                        studentCourses.Add(course.Name);
                                    }
                                }
                            }
                        }

                        // เก็บข้อมูลชื่อวิชาลงใน Dictionary โดยใช้ SID เป็น key
                        studentCourseNames[student.Sid] = studentCourses;
                    }

                    return students; // ส่งคืนข้อมูลที่มีการเชื่อมโยงข้อมูลจากทั้งสองไฟล์
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"เกิดข้อผิดพลาด: {ex.Message}");
                return new List<Student>();
            }
        }

		public async void Back()
{
    await Shell.Current.GoToAsync("HomePage");
    
}


        // Back command
       public async Task Withdraw(int courseId)
{
    // Define the path to the student JSON file
    var filePath = Path.Combine(FileSystem.AppDataDirectory, "student.json");

    // Check if the file exists
    if (File.Exists(filePath))
    {
        // Read the JSON file and deserialize into an ObservableCollection of Student objects
        var jsonData = File.ReadAllText(filePath);
        var students = JsonConvert.DeserializeObject<ObservableCollection<Student>>(jsonData);

        // Find the student who has the course registered in Term 3
        var student = students.FirstOrDefault(s => s.Year.Any(y => y.CoursesYear.Any(c => c.RegisteredTerm3.Any(t => t.Cid == courseId.ToString()))));

        if (student != null)
        {
            // Find the Year and CoursesYear object containing the Term 3 course
            var year = student.Year.FirstOrDefault(y => y.CoursesYear.Any(c => c.RegisteredTerm3.Any(t => t.Cid == courseId.ToString())));
            if (year != null)
            {
                // Find the course inside CoursesYear that has the Term 3 course
                var course = year.CoursesYear.FirstOrDefault(c => c.RegisteredTerm3.Any(t => t.Cid == courseId.ToString()));
                if (course != null)
                {
                    // Find the Term 3 course and remove it
                    var term3Course = course.RegisteredTerm3.FirstOrDefault(t => t.Cid == courseId.ToString());
                    if (term3Course != null)
                    {
                        course.RegisteredTerm3.Remove(term3Course);  // Remove the course from Term 3
                        // Save the updated students list back to the file
                        File.WriteAllText(filePath, JsonConvert.SerializeObject(students));
                        await App.Current.MainPage.DisplayAlert("Success", "Course withdrawn successfully.", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Course not found in Term 3.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Course not found for this year.", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Year not found for this student.", "OK");
            }
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Error", "Student not found or course not registered in Term 3.", "OK");
        }
    }
    else
    {
        await App.Current.MainPage.DisplayAlert("Error", "Student data file not found.", "OK");
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

            // ตรวจสอบชื่อวิชาของ student ที่มี Sid ตรงกับ _dataService.Sid
            if (studentCourseNames.ContainsKey(_dataService.Sid))
            {
                var courseNames = studentCourseNames[_dataService.Sid];
                System.Diagnostics.Debug.WriteLine($"ชื่อวิชาของ Sid {_dataService.Sid}: {string.Join(", ", courseNames)}");
            }
        }

        // Click command
        [RelayCommand]
        void Click(long id)
        {
            System.Diagnostics.Debug.WriteLine(id);
        }
    }
}
