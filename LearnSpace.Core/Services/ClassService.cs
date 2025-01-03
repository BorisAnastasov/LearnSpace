using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Class;
using LearnSpace.Core.Models.Enumerations;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Repository;
using Microsoft.EntityFrameworkCore;

namespace LearnSpace.Core.Services
{
    public class ClassService : IClassService
    {
        private readonly IRepository repository;
        public ClassService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            var course = await repository.GetByIdAsync<Course>(id);

            return course != null;
        }

        public async Task CreateClassAsync(CreateClassModel model)
        {
            var course = new Course()
            {
                TeacherId = Guid.Parse(model.TeacherId),
                Description = model.Description,
                Name = model.Name,
                GroupCapacity = model.GroupCapacity,
            };

            await repository.AddAsync(course);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteClassAsync(int id)
        {
            var studentCourses = repository.All<StudentCourse>().ToList().Where(sc => sc.CourseId == id);

            repository.DeleteRange(studentCourses);

            await repository.DeleteAsync<Course>(id);
            await repository.SaveChangesAsync();
        }

        public async Task<AllClassesViewModel> GetAllClassesAsync(string id, string? searchTerm = null, ClassSorting sorting = ClassSorting.GroupCapacityDescending, int currPage = 1, int classesPerPage = 15)
        {
            var studentId = (await repository.GetStudentAsync(id)).Id.ToString().ToLower();
            var classesToShow = repository.AllReadOnly<Course>()
                                .Where(course => !course.CourseStudents.Any(cs => cs.StudentId.ToString().ToLower() == studentId));

            if (searchTerm != null)
            {
                string normalizedSearchedTerm = searchTerm.ToLower();
                classesToShow = classesToShow
                            .Where(c => c.Name.ToLower().Contains(normalizedSearchedTerm));
            }


            classesToShow = sorting switch
            {
                ClassSorting.GroupCapacityAscending => classesToShow.OrderBy(c => c.GroupCapacity),
                ClassSorting.GroupCapacityDescending => classesToShow.OrderByDescending(c => c.GroupCapacity),
                ClassSorting.LowCapacity => classesToShow.OrderBy(c => c.GroupCapacity - c.CourseStudents.Count),
                ClassSorting.HighCapacity => classesToShow.OrderByDescending(c => c.GroupCapacity - c.CourseStudents.Count),

                _ => classesToShow.OrderBy(c => c.Id)
            };

            var allClasses = classesToShow
                                .Skip((currPage - 1) * classesPerPage)
                                .Take(classesPerPage)
                                .Select(c => new ClassInfoModel
                                {
                                    Id = c.Id,
                                    TeacherName = c.Teacher.ApplicationUser.FirstName + " " + c.Teacher.ApplicationUser.LastName,
                                    Name = c.Name,
                                    AssignmentCount = c.Assignments.Count,
                                    CurrentStudentCount = c.CourseStudents.Count,
                                    GroupCapacity = c.GroupCapacity,
                                    GroupCurrentCount = c.CourseStudents.Count,

                                }).ToList();

            var result = new AllClassesViewModel
            {
                Classes = allClasses,
            };

            result.TotalClassesCount = result.Classes.Count;

            return result;
        }

        public async Task<AllClassesViewModel> GetAllClassesForStudentAsync(string id)
        {
            var student = await repository.GetStudentAsync(id);
            var classes = new AllClassesViewModel();
            var courses = student.StudentCourses.Select(sc => sc.Course);

            foreach (var c in courses)
            {
                var classInfo = new ClassInfoModel()
                {
                    Id = c.Id,
                    TeacherName = c.Teacher.ApplicationUser.FirstName + " " + c.Teacher.ApplicationUser.LastName,
                    Name = c.Name,
                    AssignmentCount = c.Assignments.Count,
                    CurrentStudentCount = c.CourseStudents.Count,
                    GroupCapacity = c.GroupCapacity
                };
                classes.Classes.Add(classInfo);
            }
            classes.TotalClassesCount = classes.Classes.Count;

            return classes;
        }

        public async Task<AllClassesViewModel> GetAllClassesForTeacherAsync(string userId)
        {
            var teacher = await repository.GetTeacherAsync(userId);
            var classes = new AllClassesViewModel();
            var courses = teacher.Courses;

            foreach (var c in courses)
            {
                var classInfo = new ClassInfoModel()
                {
                    Id = c.Id,
                    TeacherName = c.Teacher.ApplicationUser.FirstName + " " + c.Teacher.ApplicationUser.LastName,
                    Name = c.Name,
                    AssignmentCount = c.Assignments.Count,
                    CurrentStudentCount = c.CourseStudents.Count,
                    GroupCapacity = c.GroupCapacity
                };
                classes.Classes.Add(classInfo);
            }
            classes.TotalClassesCount = classes.Classes.Count;

            return classes;
        }

        public CreateClassModel GetCreateClassModel(string id)
        {
            var teacherId = repository.GetTeacherAsync(id).Result.Id.ToString().ToLower();

            var model = new CreateClassModel()
            {
                TeacherId = teacherId,
            };

            return model;
        }

        public async Task JoinClassAsync(string userId, int id)
        {
            var course = await repository.GetByIdAsync<Course>(id);

            if (course.GroupCapacity - course.CourseStudents.Count <= 0)
            {
                return;
            }

            var student = await repository.GetStudentAsync(userId);
            var studentCourse = new StudentCourse
            {
                StudentId = student.Id,
                CourseId = id
            };

            await repository.AddAsync(studentCourse);
            await repository.SaveChangesAsync();
        }

        public async Task LeaveClassAsync(string userId, int classId)
        {
            var student = await repository.GetStudentAsync(userId);
            var studentCourse = student.StudentCourses.First(sc => sc.CourseId == classId);

            repository.Delete(studentCourse);
            await repository.SaveChangesAsync();
        }
    }
}
