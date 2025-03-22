using MauiMiniProject.Pages;

namespace MauiMiniProject.Pages
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            // นำทางไปยังหน้าโปรไฟล์
            await Shell.Current.GoToAsync("ProfilePage");
        }

        private async void OnRegisteredCoursesButtonClicked(object sender, EventArgs e)
        {
            // นำทางไปยังหน้าดูวิชาที่ลงทะเบียน
            await Shell.Current.GoToAsync("RegisteredCoursesPage");
        }

        private async void OnSearchCoursesButtonClicked(object sender, EventArgs e)
        {
            // นำทางไปยังหน้าค้นหาวิชา
            await Shell.Current.GoToAsync("SearchCoursesPage");
        }

        private async void OnWithdrawCourseButtonClicked(object sender, EventArgs e)
        {
            // นำทางไปยังหน้าถอนรายวิชา
            await Shell.Current.GoToAsync("WithdrawCoursePage");
        }
    }
}
