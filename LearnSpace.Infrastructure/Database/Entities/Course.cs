using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LearnSpace.Infrastructure.Database.Entities.Account;
using static LearnSpace.Common.DataConstants.Course;

namespace LearnSpace.Infrastructure.Database.Entities
{
    public class Course
    {
        public Course()
        {
            this.CourseStudents = new HashSet<StudentCourse>();
            this.Assignments = new HashSet<Assignment>();
            this.Grades = new HashSet<Grade>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLen)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(DescriptionMaxLen)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int GroupCapacity { get; set; }

        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual ICollection<StudentCourse> CourseStudents { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }

}
