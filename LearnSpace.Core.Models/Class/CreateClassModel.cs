using static LearnSpace.Common.MessageConstants;
using static LearnSpace.Common.DataConstants.Course;
using System.ComponentModel.DataAnnotations;

namespace LearnSpace.Core.Models.Class
{
    public class CreateClassModel
    {
        public string TeacherId { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(NameMaxLen, MinimumLength = NameMinLen, ErrorMessage = LengthMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(DescriptionMaxLen, MinimumLength = DescriptionMinLen, ErrorMessage = LengthMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [Range(GroupCapacityMin, GroupCapacityMax,ErrorMessage =NumberMessage)]
        public int GroupCapacity { get; set; }

    }
}
