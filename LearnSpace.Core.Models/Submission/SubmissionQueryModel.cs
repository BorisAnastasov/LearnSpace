namespace LearnSpace.Core.Models.Submission
{
    public class SubmissionQueryModel
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string StudentName { get; set; }
        public string AssignmentTitle { get; set; }
        public string FileName { get; set; }
        public string SubmittedOn { get; set; }

    }
}
