namespace LearnSpace.Core.Models.Grade
{
    public class GradeCourseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public List<GradeServiceModel> Grades { get; set; } = new List<GradeServiceModel>();
    }
}
