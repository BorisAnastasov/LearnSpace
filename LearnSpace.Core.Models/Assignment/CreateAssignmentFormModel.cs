using static LearnSpace.Common.MessageConstants;
using static LearnSpace.Common.DataConstants.Assignment;
using System.ComponentModel.DataAnnotations;
using LearnSpace.Common;

namespace LearnSpace.Core.Models.Assignment
{
    public class CreateAssignmentFormModel
    {
        [Required(ErrorMessage =RequiredMessage)]
        [StringLength(TitleMaxLen, MinimumLength = TitleMinLen, ErrorMessage = LengthMessage)]
        public string Title { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(DescriptionMaxLen, MinimumLength = DescriptionMinLen, ErrorMessage = LengthMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = MessageConstants.InvalidDateTimeFormat)]
        public string DueDate { get; set; }
        public int ClassId { get; set; }

    }
}
