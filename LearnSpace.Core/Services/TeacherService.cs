using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Teacher;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;

namespace LearnSpace.Core.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository repository;
        public TeacherService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<bool> ClassExistsByIdAsync(int id)
        {
            var course = await repository.GetByIdAsync<Course>(id);

            return course != null;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            var result = await repository.GetByIdAsync<ApplicationUser>(Guid.Parse(id));

            return result.Student != null;
        }

        public async Task<GradeBookViewModel> GetGradeBookByClassAsync(string userId, int classId)
        {
            var teacher = await repository.GetTeacherAsync(userId);
            var course = await repository.GetByIdAsync<Course>(classId);

            var list = teacher.Courses
                                .First(c => c.Id == classId)
                                .CourseStudents.Select(sc => new StudentGradesServiceModel
                                {
                                    StudentId = sc.Student.Id,
                                    StudentName = sc.Student.ApplicationUser.FirstName + " " + sc.Student.ApplicationUser.LastName,
                                    Grades = sc.Student.Grades.Where(g => g.CourseId == classId).Select(g => new GradeServiceModel
                                    {
                                        Id = g.Id,
                                        Score = g.Score
                                    }).ToList()
                                }).ToList();
            var model = new GradeBookViewModel
            {
                List = list,
                ClassId = classId
            };
            return model;
        }

        public async Task<TeacherDashboardModel> GetTeacherDashboardInformationAsync(string id)
        {
            var teacher = await repository.GetTeacherAsync(id);

            var model = new TeacherDashboardModel
            {
                FullName = teacher.ApplicationUser.FirstName + " " + teacher.ApplicationUser.LastName,
                TotalStudentsEnrolled = teacher.Courses.Sum(c => c.CourseStudents.Count),
                AssignmentCount = teacher.Courses.SelectMany(c => c.Assignments).Count(),
                WaitingSubmissions = teacher.Courses.SelectMany(c => c.Assignments)
                                        .SelectMany(a => a.Submissions)
                                        .Count()
            };

            return model;
        }
    }
}
