using LearnSpace.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSpace.Infrastructure.Database.Configuration
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasData(SeedGrades());
        }

        private static List<Grade> SeedGrades() 
        {
            var grades = new List<Grade>();

            //1
            var grade = new Grade() 
            {
                Id = 1,
                Score = 4,
                DateGraded = DateTime.Now,
                StudentId = Guid.Parse("5c07f155-602e-403b-bb86-5a786814f575"),
                Description = "You can do more.",
                CourseId = 1
            };

            grades.Add(grade);

            //2
            grade = new Grade()
            {
                Id = 2,
                Score = 3,
                DateGraded = DateTime.Now,
                StudentId = Guid.Parse("18e76084-b8a6-4e78-bd26-143f33a05eb8"),
                Description = "You can do more.",
                CourseId = 2
            };

            grades.Add(grade);

            //3
            grade = new Grade()
            {
                Id = 3,
                Score = 5,
                DateGraded = DateTime.Now,
                StudentId = Guid.Parse("c6903087-71e5-41ba-80be-ed119b7902fc"),
                Description = "You can do more.",
                CourseId = 3
            };

            grades.Add(grade);

            //4
            grade = new Grade()
            {
                Id = 4,
                Score = 6,
                DateGraded = DateTime.Now,
                StudentId = Guid.Parse("f4aa693d-305e-426b-950c-d02a8ca8b56f"),
                Description = "You can do more.",
                CourseId = 4
            };

            grades.Add(grade);

            //5
			grade = new Grade()
			{
				Id = 5,
				Score = 6,
				DateGraded = DateTime.Now,
				StudentId = Guid.Parse("bb5432a1-ea56-450b-9db6-f7349faf28a6"),
                Description = "You can do more.",
                CourseId = 5
            };

			grades.Add(grade);

			return grades;
        }
    }
}
