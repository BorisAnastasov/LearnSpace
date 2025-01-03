using LearnSpace.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSpace.Infrastructure.Database.Configuration
{
    public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.HasData(SeedSubmissions());
        }
        private static List<Submission> SeedSubmissions()
        {
            var submissions = new List<Submission>();

            var file1Content = FileToByteArray("\\uploads\\submissions\\task1.txt");
            var file2Content = FileToByteArray("\\uploads\\submissions\\task1.txt");
            var file3Content = FileToByteArray("\\uploads\\submissions\\task1.txt");
            var file4Content = FileToByteArray("\\uploads\\submissions\\task1.txt");
            var file5Content = FileToByteArray("\\uploads\\submissions\\task1.txt");

            // 1
            submissions.Add(new Submission()
            {
                Id = 1,
                AssignmentId = 2,
                StudentId = Guid.Parse("5c07f155-602e-403b-bb86-5a786814f575"),
                FileType = "text/plain",
                FileName ="task1.txt",
                FileContent = file1Content, 
                SubmittedOn = DateTime.UtcNow,
            });

            // 2
            submissions.Add(new Submission()
            {
                Id = 2,
                AssignmentId = 4,
                StudentId = Guid.Parse("18e76084-b8a6-4e78-bd26-143f33a05eb8"),
                FileContent = file2Content,
                FileType = "text/plain",
                FileName = "task2.txt",
                SubmittedOn = DateTime.UtcNow,
            });

            // 3
            submissions.Add(new Submission()
            {
                Id = 3,
                AssignmentId = 6,
                StudentId = Guid.Parse("c6903087-71e5-41ba-80be-ed119b7902fc"),
                FileContent = file3Content,
                FileType = "text/plain",
                FileName = "task3.txt",
                SubmittedOn = DateTime.UtcNow,
            });

            // 4
            submissions.Add(new Submission()
            {
                Id = 4,
                AssignmentId = 8,
                StudentId = Guid.Parse("f4aa693d-305e-426b-950c-d02a8ca8b56f"),
                FileContent = file4Content,
                FileType = "text/plain",
                FileName = "task4.txt",
                SubmittedOn = DateTime.UtcNow,
            });

            // 5
            submissions.Add(new Submission()
            {
                Id = 5,
                AssignmentId = 10,
                StudentId = Guid.Parse("bb5432a1-ea56-450b-9db6-f7349faf28a6"),
                FileContent = file5Content,
                FileType = "text/plain",
                FileName = "task5.txt",
                SubmittedOn = DateTime.UtcNow,
            });

            return submissions;
        }

        private static byte[] FileToByteArray(string relativePath)
        {
            var rootPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName, "wwwroot");
            relativePath = relativePath.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var filePath = Path.Combine(rootPath, relativePath);

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }

            throw new FileNotFoundException($"File not found: {filePath}");
        }

    }
}
