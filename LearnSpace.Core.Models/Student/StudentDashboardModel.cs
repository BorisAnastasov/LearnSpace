namespace LearnSpace.Core.Models.Student
{
    public class StudentDashboardModel
    {
        public string FullName { get; set; }
        public double Success { get; set; } = 0;
        public int GradeCount { get; set; }
        public int AssignmentCount { get; set; }
        public int ClassCount { get; set; }
        public List<ChartSuccessModel> ChartData { get; set; }

    }
}
