namespace Employee_leave_system.Models
{
    public class AppliedEmployeeLeaves
    {
        public int UserId { get; set; }
        public int ApplicationId { get; set; }
        public string TypeOfLeave { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DateOfApplication { get; set; }
        public DateTime LeaveFromDt { get; set; }
        public DateTime LeaveToDt { get; set; }

    }
}
