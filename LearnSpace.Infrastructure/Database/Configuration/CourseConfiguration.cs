using LearnSpace.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSpace.Infrastructure.Database.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasData(SeedCourses());
        }

        private static List<Course> SeedCourses() 
        {
            var courses = new List<Course>();

            var course = new Course()
            { 
                Id = 1,
                Name = "Geometry",
                Description =  "In this course you can learn the basic concepts of the 3D world with Math. Learn Geometry today!",
                TeacherId = Guid.Parse("5e846435-c18b-4df9-b4a2-b6d9d0414694"),
                GroupCapacity = 20
            };

            courses.Add(course);

            course = new Course()
            {
                Id = 2,
                Name = "Algebra",
                Description = "In this course you can learn the basic concepts of numbers around us. Learn Algebra today!",
                TeacherId = Guid.Parse("5e846435-c18b-4df9-b4a2-b6d9d0414694"),
                GroupCapacity = 25
            };

            courses.Add(course);

            course = new Course()
            {
                Id = 3,
                Name = "Modern History",
                Description = "In this course you can learn how the new empires are formed and passed to the next generations. Join us!",
                TeacherId = Guid.Parse("047e49c7-8466-4419-88e3-1b6f7107d247"),
                GroupCapacity = 20
            };

            courses.Add(course);

            course = new Course()
            {
                Id = 4,
                Name = "Ancient History",
                Description = "In this course you can learn how the people in the ancient times have lived. Interested? Let's learn together!",
                TeacherId = Guid.Parse("047e49c7-8466-4419-88e3-1b6f7107d247"),
                GroupCapacity = 30
            };

            courses.Add(course);

            course = new Course()
            {
                Id = 5,
                Name = "Middle Ages",
                Description = "In this course you can learn how hard were the middle ages and why they are important. Join today!",
                TeacherId = Guid.Parse("047e49c7-8466-4419-88e3-1b6f7107d247"),
                GroupCapacity = 20
            };

            courses.Add(course);

            return courses;
        }
    }
}
