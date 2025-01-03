using LearnSpace.Infrastructure.Database.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSpace.Infrastructure.Database.Configuration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasData(SeedStudents());
        }

        private static List<Teacher> SeedStudents() 
        {
            var teachers = new List<Teacher>();

            var teacher = new Teacher()
            { 
                Id = Guid.Parse("5e846435-c18b-4df9-b4a2-b6d9d0414694"),
                SubjectSpecialization = "Math",
                ApplicationUserId = Guid.Parse("bdc70ff8-a02a-428f-ad1c-b5ba645a45e1")
            };

            teachers.Add(teacher);

            teacher = new Teacher()
            {
                Id = Guid.Parse("047e49c7-8466-4419-88e3-1b6f7107d247"),
                SubjectSpecialization = "History",
                ApplicationUserId = Guid.Parse("bc5f8df5-6115-4344-897b-73e185df4bff")
            };

            teachers.Add(teacher);

            return teachers;
        }
    }
}
