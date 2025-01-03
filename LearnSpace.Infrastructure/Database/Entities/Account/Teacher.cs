using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LearnSpace.Common.DataConstants.Teacher;

namespace LearnSpace.Infrastructure.Database.Entities.Account
{
    public class Teacher
    {
        public Teacher()
        {
            Id = Guid.NewGuid();
            this.Courses = new HashSet<Course>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(SubjectSpecializationMaxLen)]
        public string SubjectSpecialization { get; set; } = string.Empty;
        public virtual ICollection<Course> Courses { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public Guid ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }

}
