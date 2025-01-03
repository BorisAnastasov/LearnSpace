using LearnSpace.Infrastructure.Database.Configuration;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnSpace.Infrastructure.Database
{
    public class LearnSpaceDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public LearnSpaceDbContext(DbContextOptions<LearnSpaceDbContext> options)
        : base(options) 
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }

        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<Assignment> Assignments { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<StudentCourse> StudentsCourses { get; set; } = null!;
        public virtual DbSet<Submission> Submissions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StudentCourse>()
                   .HasKey(sc => new
                   {
                       sc.StudentId,
                       sc.CourseId
                   });

            builder.Entity<Student>()
                    .HasMany(s => s.StudentCourses)
                    .WithOne(sc => sc.Student)
                    .HasForeignKey(sc => sc.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                    .HasMany(s => s.Submissions)
                    .WithOne(sc => sc.Student)
                    .HasForeignKey(sc => sc.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Submission>()
                    .HasOne(s => s.Assignment)
                    .WithMany   (a => a.Submissions)
                    .HasForeignKey(s => s.AssignmentId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Student>()
                    .HasMany(s => s.Grades)
                    .WithOne(s => s.Student)
                    .HasForeignKey(g=>g.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Grade>()
                    .HasOne(g => g.Course)
                    .WithMany(c=>c.Grades)
                    .HasForeignKey(g => g.CourseId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                    .HasOne(a => a.Student)
                    .WithOne(s=>s.ApplicationUser)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                    .HasOne(a => a.Teacher)
                    .WithOne(s => s.ApplicationUser)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Course>()
                    .HasOne(c => c.Teacher)
                    .WithMany(t => t.Courses)
                    .HasForeignKey(c => c.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Course>()
                    .HasMany(c => c.Grades)
                    .WithOne(g => g.Course)
                    .HasForeignKey(g => g.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Course>()
                    .HasMany(c => c.CourseStudents)
                    .WithOne(sc => sc.Course)
                    .HasForeignKey(sc => sc.CourseId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Course>()
                    .HasMany(c => c.Assignments)
                    .WithOne(a => a.Course)
                    .HasForeignKey(a => a.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

            //Seeding

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new TeacherConfiguration());
            builder.ApplyConfiguration(new StudentConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new StudentCourseConfiguration());
            builder.ApplyConfiguration(new AssignmentConfiguration());
            builder.ApplyConfiguration(new SubmissionConfiguration());
            builder.ApplyConfiguration(new GradeConfiguration());

            base.OnModelCreating(builder);
        }

    }
}
