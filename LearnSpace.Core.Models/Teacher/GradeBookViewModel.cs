namespace LearnSpace.Core.Models.Teacher
{
    public class GradeBookViewModel
    {
        public int ClassId { get; set; }
        public List<StudentGradesServiceModel> List { get; set; }
    }
}
