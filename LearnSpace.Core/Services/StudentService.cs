using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Student;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;

namespace LearnSpace.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository repository;
        public StudentService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            var result = await repository.GetByIdAsync<ApplicationUser>(Guid.Parse(id));

            return result.Student != null;
        }

        public async Task<StudentDashboardModel> GetStudentDashboardInformationAsync(string id)
        {
            var student = await repository.GetStudentAsync(id);

            var model = new StudentDashboardModel();

            model.FullName = student.ApplicationUser.FirstName + " " + student.ApplicationUser.LastName;
            if (student.StudentCourses.Any(sc => sc.Course.Grades.Any()))
            {
                model.Success = student.StudentCourses.SelectMany(sc => sc.Course.Grades.Where(g => g.StudentId == student.Id)).ToList().Average(g => g.Score);
            }
            model.GradeCount = student.StudentCourses.SelectMany(sc => sc.Course.Grades.Where(g => g.StudentId == student.Id)).Count();
            model.ClassCount = student.StudentCourses.Count();
            model.AssignmentCount = student.StudentCourses.Sum(c => c.Course.Assignments.Count);

            var grades = await repository
                .AllReadOnly<Grade>(a => a.StudentId == student.Id)
                .Select(g => new { g.DateGraded, g.Score })
                .ToListAsync();

            var averageSuccessData = grades
                .GroupBy(g => g.DateGraded.Date)
                .Select(g => new ChartSuccessModel
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    AverageGrade = g.Average(x => x.Score)
                })
                .OrderBy(x => x.Date)
                .ToList();

            model.ChartData = averageSuccessData;

            return model;
        }
    }
}
