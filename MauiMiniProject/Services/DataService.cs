// DataService.cs
namespace MauiMiniProject.Services
{
    public class DataService : Iservice
    {
        public long Sid { get; set; }
		public string name { get; set; }
    

	public DataService()
        {
            // กำหนดค่าเริ่มต้น
            Sid = 0;
            name = string.Empty;
        }
	}
}
