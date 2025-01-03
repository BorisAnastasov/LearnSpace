using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Assignment;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Identity;
using static LearnSpace.Common.Constants;

namespace LearnSpace.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IRepository repository;
        public AssignmentService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task CreateAssignment(CreateAssignmentFormModel model)
        {
            DateTime parsedDueDate;
            const string dateFormat = "yyyy-MM-dd";
            if (!DateTime.TryParseExact(model.DueDate, dateFormat, null, System.Globalization.DateTimeStyles.None, out parsedDueDate))
            {
                throw new ArgumentException($"Invalid date format. Expected format is {dateFormat}.");
            }

            var assignment = new Assignment()
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = parsedDueDate,
                CourseId = model.ClassId
            };

            await repository.AddAsync(assignment);
            await repository.SaveChangesAsync();
        }

		public async Task<int> DeleteAssignment(int id)
		{
            var assignment = await repository.GetByIdAsync<Assignment>(id);
            var classId = assignment.CourseId;

            await repository.DeleteAsync<Assignment>(id);
            await repository.SaveChangesAsync();

            return classId;
		}

		public async Task<bool> ExistsByIdAsync(int id)
        {
            var assignment = await repository.GetByIdAsync<Assignment>(id);

            return assignment != null;
        }

        public async Task<AllAssignmentsViewModel> GetAllAssignmentsAsync(string userId)
        {
            var student = await repository.GetStudentAsync(userId);

            var allAssignments = student.StudentCourses
                                .SelectMany(sc => sc.Course.Assignments).Select(a => new AssignmentServiceModel
                                {
                                    Id = a.Id,
                                    DueDate = a.DueDate.ToString(DateFormat),
                                    Title = a.Title,
                                    IsSubmitted = a.Submissions.Any(s => s.StudentId == student.Id),

                                }).ToList();

            var result = new AllAssignmentsViewModel()
            {
                Assignments = allAssignments
            };

            return result;
        }

        public async Task<AssignmentsClassViewModel> GetAllAssignmentsByClassForStudentAsync(string userId, int classId)
        {
            var student = await repository.GetStudentAsync(userId);
            var course = await repository.GetByIdAsync<Course>(classId);

            var allAssignments = student.StudentCourses
                                        .First(sc => sc.Course.Id == classId)
                                        .Course.Assignments.Select(a => new AssignmentServiceModel
                                        {
                                            Id = a.Id,
                                            DueDate = a.DueDate.ToString(DateFormat),
                                            Title = a.Title,
                                            IsSubmitted = a.Submissions.Any(s => s.StudentId == student.Id),
                                        }).ToList();

            var result = new AssignmentsClassViewModel()
            {
                ClassName = course.Name,
                Assignments = allAssignments
            };

            return result;

        }

        public async Task<AllAssignmentsForTeacherViewModel> GetAllAssignmentsByClassForTeacherAsync(string userId, int classId)
        {
            var teacher = await repository.GetTeacherAsync(userId);
            var course = await repository.GetByIdAsync<Course>(classId);

            var assignments = teacher.Courses
                                        .First(c => c.Id == classId)
                                        .Assignments.Select(a => new AssignmentForTeacherModel
                                        {
                                            Id = a.Id,
                                            DueDate = a.DueDate.ToString(DateFormat),
                                            Title = a.Title,
                                        }).ToList();

            var result = new AllAssignmentsForTeacherViewModel()
            {
                Assignments = assignments,
                ClassId = classId
            };

            return result;
        }

        public async Task<AssignmentInfoViewModel> GetAssignmentInfoAsync(string userId, int assignmentId)
        {
            var assignment = await repository.GetByIdAsync<Assignment>(assignmentId);
            var student = await repository.GetStudentAsync(userId);
            var model = new AssignmentInfoViewModel
            {
                Id = assignmentId,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate.ToString(DateFormat),
                ClassName = assignment.Course.Name,
                TeacherName = assignment.Course.Teacher.ApplicationUser.FirstName + " " + assignment.Course.Teacher.ApplicationUser.LastName,
                IsSubmitted = assignment.Submissions.Any(s => s.StudentId == student.Id),
                SubmissionDate = assignment.Submissions.Any(s => s.StudentId == student.Id) ?
                                                assignment.Submissions
                                                        .First(s => s.StudentId == student.Id)
                                                        .SubmittedOn
                                                        .ToString(DateFormat) :
                                                string.Empty
            };

            return model;
        }

        public async Task<AssignmentsInfoForTeacherViewModel> GetAssignmentInfoForTeacherAsync(int assignmentId)
        {
            var assignment = await repository.GetByIdAsync<Assignment>(assignmentId);
            var model = new AssignmentsInfoForTeacherViewModel
            {
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate.ToString(DateFormat),
                ClassName = assignment.Course.Name
            };

            return model;
        }

        public async Task<bool> ClassExistsByIdAsync(int id)
        {
            var course = await repository.GetByIdAsync<Course>(id);

            return course != null;
        }
    }
}
