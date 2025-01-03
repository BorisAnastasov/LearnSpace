using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Grade;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;

namespace LearnSpace.Core.Services
{
    public class GradeService : IGradeService
    {
        private readonly IRepository repository;
        public GradeService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            var grade = await repository.GetByIdAsync<Grade>(id);

            return grade != null;
        }

        public async Task CreateGradeAsync(CreateGradeViewModel model)
        {
            var grade = new Grade()
            {
                CourseId = model.CourseId,
                StudentId = Guid.Parse(model.StudentId),
                Score = model.Score,
                DateGraded = DateTime.Now,
                Description = model.Description,
            };

            await repository.AddAsync(grade);
            await repository.SaveChangesAsync();
        }

        public async Task<int> DeleteGradeAsync(int id)
        {
            var classId = repository.GetByIdAsync<Grade>(id).Result.CourseId;
            await repository.DeleteAsync<Grade>(id);
            await repository.SaveChangesAsync();

            return classId;
        }

        public async Task<CreateGradeViewModel> GetAddGradeModelAsync(int classId, string userId)
        {
            var student = await repository.GetByIdAsync<Infrastructure.Database.Entities.Account.Student>(Guid.Parse(userId));
            var course = await repository.GetByIdAsync<Course>(classId);
            var model = new CreateGradeViewModel
            {
                CourseId = classId,
                StudentId = student.Id.ToString().ToLower(),
                CourseName = course.Name,
                StudentName = student.ApplicationUser.FirstName + " " + student.ApplicationUser.LastName
            };

            return model;
        }

        public async Task<List<GradeCourseViewModel>> GetAllGradesAsync(string id)
        {
            var student = await repository.GetStudentAsync(id);
            var list = new List<GradeCourseViewModel>();
            var gradeCourse = new GradeCourseViewModel();
            var courses = student.StudentCourses.Select(sc => sc.Course).ToList();

            foreach (var course in courses)
            {
                if (course.Grades.Any(g => g.StudentId == student.Id))
                {
                    gradeCourse.Grades = course.Grades.Where(g => g.StudentId == student.Id)
                                        .Select(g => new GradeServiceModel
                                        {
                                            Score = g.Score,
                                            Id = g.Id
                                        }).ToList();

                }
                gradeCourse.Name = course.Name;
                list.Add(gradeCourse);
                gradeCourse = new GradeCourseViewModel();

            }

            return list;
        }

        public async Task<GradeInfoViewModel> GetGradeInfoAsync(int id)
        {
            var grade = await repository.GetByIdAsync<Grade>(id);

            var gradeInfo = new GradeInfoViewModel();

            gradeInfo.Id = id;
            gradeInfo.CourseName = grade.Course.Name;
            gradeInfo.Score = grade.Score;
            gradeInfo.Teacher = grade.Course.Teacher.ApplicationUser.FirstName + " " + grade.Course.Teacher.ApplicationUser.LastName;
            gradeInfo.Description = grade.Description;
            gradeInfo.CourseId = grade.Course.Id;

            return gradeInfo;

        }

        public async Task<bool> ClassExistsByIdAsync(int id)
        {
            var course = await repository.GetByIdAsync<Course>(id);

            return course != null;
        }

        public async Task<bool> UserExistsByIdAsync(string studentId)
        {
            var student = await repository.GetByIdAsync<Student>(Guid.Parse(studentId));

            return student != null;
        }
    }
}
