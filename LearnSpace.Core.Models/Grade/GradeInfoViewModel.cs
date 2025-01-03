namespace LearnSpace.Core.Models.Grade
{
    public class GradeInfoViewModel
    {
        public int Id { get; set; }
        public double Score { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string Description { get; set; }
        public string Teacher { get; set; } = string.Empty;
    }
}
