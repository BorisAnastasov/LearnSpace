using System.ComponentModel.DataAnnotations;

namespace LearnSpace.Infrastructure.Database.Entities.Account
{
    public class Student
    {
        public Student()
        {
            Id = Guid.NewGuid();
            this.StudentCourses = new HashSet<StudentCourse>();
            this.Submissions = new HashSet<Submission>();
            this.Grades = new HashSet<Grade>();
            
        }

        [Key]
        public Guid Id { get; set; }
        public virtual IEnumerable<StudentCourse> StudentCourses { get; set; } 
        public virtual IEnumerable<Submission> Submissions { get; set; } 
        public virtual IEnumerable<Grade> Grades { get; set; } 

        public Guid ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
