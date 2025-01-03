using System.ComponentModel.DataAnnotations;
using static LearnSpace.Common.MessageConstants;
using static LearnSpace.Common.DataConstants.Grade;

namespace LearnSpace.Core.Models.Grade
{
    public class CreateGradeViewModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [Range(ScoreMin,ScoreMax,ErrorMessage =NumberMessage)]
        public int Score { get; set; }
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(DescriptionMaxLen,
                MinimumLength = DescriptionMinLen,
                ErrorMessage = LengthMessage)]
        public string Description { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }

    }
}
