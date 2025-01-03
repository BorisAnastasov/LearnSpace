namespace LearnSpace.Core.Models.Submission
{
    public class SubmissionFileModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
    }
}
