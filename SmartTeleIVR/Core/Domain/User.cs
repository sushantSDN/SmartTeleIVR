namespace SmartTeleIVR.Core.Domain
{
    public class User
    {
        public string UniqueID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class AppointmentDetails
    {
        public string AppointmentID { get; set; }
        public string UserID { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; }
    }

    public static class HardcodedData
    {
        public static List<User> GetUsers()
        {
            return new List<User>
            {
                new User { UniqueID = "001", Name = "Sushant Buche", PhoneNumber = "+918698999259", Address = "123 Main St, NY" },
                new User { UniqueID = "002", Name = "Jane Smith", PhoneNumber = "0987654321", Address = "456 Elm St, CA" }
            };
        }

        public static List<AppointmentDetails> GetAppointments()
        {
            var users = GetUsers();
            var now = DateTime.Now;

            return new List<AppointmentDetails>
            {
                new AppointmentDetails { AppointmentID = "A1001", UserID = users[0].UniqueID, AppointmentTime = now.AddHours(1), Status = "Scheduled" },
                new AppointmentDetails { AppointmentID = "A1002", UserID = users[1].UniqueID, AppointmentTime = now.AddHours(2), Status = "Confirmed" }
            };
        }
    }
}
