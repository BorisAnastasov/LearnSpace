namespace LearnSpace.Core.Models.Teacher
{
    public class TeacherDashboardModel
    {
        public string FullName { get; set; }
        public int TotalStudentsEnrolled { get; set; }
        public int AssignmentCount { get; set; }
        public int WaitingSubmissions { get; set; }
    }
}
