namespace LearnSpace.Core.Models.Assignment
{
    public class AssignmentInfoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string FilePath { get; set; }
        public bool IsSubmitted { get; set; }
        public string SubmissionDate { get; set; }
		public List<string> Errors { get; set; } = new List<string>();

	}
}
