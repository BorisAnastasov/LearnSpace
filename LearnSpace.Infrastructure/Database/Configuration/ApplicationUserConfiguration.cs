using LearnSpace.Infrastructure.Database.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSpace.Infrastructure.Database.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(SeedApplicationUsers());
        }

        private static List<ApplicationUser> SeedApplicationUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var users = new List<ApplicationUser>();

            //Admin (1)
            var user = new ApplicationUser()
            {
                Id = Guid.Parse("2d522f0f-1d26-429e-8bef-0098f10d96e9"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@abv.bg",
                NormalizedEmail = "ADMIN@ABV.BG",
                FirstName = "Admin",
                LastName = "Adminov",
                DateOfBirth = new DateTime(1980, 5, 10)
            };

            user.PasswordHash = hasher.HashPassword(user, "123456");
            users.Add(user);

            // Teachers (2)

            //1
            user = new ApplicationUser()
            {
                Id = Guid.Parse("bdc70ff8-a02a-428f-ad1c-b5ba645a45e1"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "teacher1",
                NormalizedUserName = "TEACHER1",
                Email = "teacher1@abv.bg",
                NormalizedEmail = "TEACHER1@ABV.BG",
                FirstName = "Ivan",
                LastName = "Ivanov",
                DateOfBirth = new DateTime(1980, 5, 10),
            };

            user.PasswordHash = hasher.HashPassword(user, "123456");
            users.Add(user);

            //2
            user = new ApplicationUser()
            {
                Id = Guid.Parse("bc5f8df5-6115-4344-897b-73e185df4bff"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "teacher2",
                NormalizedUserName = "TEACHER2",
                Email = "teacher2@abv.bg",
                NormalizedEmail = "TEACHER2@ABV.BG",
                FirstName = "Grigor",
                LastName = "Georgiev",
                DateOfBirth = new DateTime(1982, 8, 21),
            };

            user.PasswordHash = hasher.HashPassword(user, "123456");
            users.Add(user);

            // Students (5)

            //1
            user = new ApplicationUser()
            {
                Id = Guid.Parse("a52dc824-b577-4862-ac67-29d391116793"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "student1",
                NormalizedUserName = "STUDENT1",
                Email = "student1@abv.bg",
                NormalizedEmail = "STUDENT1@ABV.BG",
                FirstName = "Vladislav",
                LastName = "Popov",
                DateOfBirth = new DateTime(2005, 3, 14),
            };

            user.PasswordHash = hasher.HashPassword(user, "123456");
            users.Add(user);

            //2
            user = new ApplicationUser()
            {
                Id = Guid.Parse("08d20ff4-ecdd-4b8a-8142-4cf42ee6adc6"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "student2",
                NormalizedUserName = "STUDENT2",
                Email = "student2@abv.bg",
                NormalizedEmail = "STUDENT2@ABV.BG",
                FirstName = "Diana",
                LastName = "Atanasova",
                DateOfBirth = new DateTime(2006, 7, 19),
            };

            user.PasswordHash = hasher.HashPassword(user, "123456");
            users.Add(user);

            //3
            user = new ApplicationUser()
            {
                Id = Guid.Parse("3cc698b0-736e-490a-97e3-3f343bf8bfd8"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "student3",
                NormalizedUserName = "STUDENT3",
                Email = "student3@abv.bg",
                NormalizedEmail = "STUDENT3@ABV.BG",
                FirstName = "Ema",
                LastName = "Ivanova",
                DateOfBirth = new DateTime(2005, 10, 22),
            };

            user.PasswordHash = hasher.HashPassword(user, "123456");
            users.Add(user);

            //4
            user = new ApplicationUser()
            {
                Id = Guid.Parse("ebdc00b8-7106-4cbd-a482-da93c40103d3"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "student4",
                NormalizedUserName = "STUDENT4",
                Email = "student4@abv.bg",
                NormalizedEmail = "STUDENT4@ABV.BG",
                FirstName = "Alex",
                LastName = "Georgiev",
                DateOfBirth = new DateTime(2006, 2, 3)
            };
            user.PasswordHash = hasher.HashPassword(user, "123456");
            users.Add(user);


            //5
            user = new ApplicationUser()
            {
                Id = Guid.Parse("267a7709-17b4-413c-9026-a6f365d59731"),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "student5",
                NormalizedUserName = "STUDENT5",
                Email = "student5@abv.bg",
                NormalizedEmail = "STUDENT5@ABV.BG",
                FirstName = "Lili",
                LastName = "Samardjieva",
                DateOfBirth = new DateTime(2006, 3, 5)
            };
            user.PasswordHash = hasher.HashPassword(user, "123456");
            users.Add(user);

            return users;
        }
    }
}
