using LearnSpace.Infrastructure.Database.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSpace.Infrastructure.Database.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasData(SeedStudents());
        }

        private static List<Student> SeedStudents()
        {
            var students = new List<Student>();

            //1
            var student = new Student()
            {
                Id = Guid.Parse("5c07f155-602e-403b-bb86-5a786814f575"),
                ApplicationUserId = Guid.Parse("a52dc824-b577-4862-ac67-29d391116793")
            };

            students.Add(student);
            //2
            student = new Student()
            {
                Id = Guid.Parse("18e76084-b8a6-4e78-bd26-143f33a05eb8"),
                ApplicationUserId = Guid.Parse("08d20ff4-ecdd-4b8a-8142-4cf42ee6adc6")
            };

            students.Add(student);
            //3
            student = new Student()
            {
                Id = Guid.Parse("c6903087-71e5-41ba-80be-ed119b7902fc"),
                ApplicationUserId = Guid.Parse("3cc698b0-736e-490a-97e3-3f343bf8bfd8")
            };

            students.Add(student);
            //4
            student = new Student()
            {
                Id = Guid.Parse("f4aa693d-305e-426b-950c-d02a8ca8b56f"),
                ApplicationUserId = Guid.Parse("ebdc00b8-7106-4cbd-a482-da93c40103d3")
            };

            students.Add(student);

            //5
            student = new Student()
            {
                Id = Guid.Parse("bb5432a1-ea56-450b-9db6-f7349faf28a6"),
                ApplicationUserId = Guid.Parse("267a7709-17b4-413c-9026-a6f365d59731")
            };

            students.Add(student);

            return students;
        }
    }
}
