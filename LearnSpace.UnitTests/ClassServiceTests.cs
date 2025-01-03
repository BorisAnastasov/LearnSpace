using LearnSpace.Core.Models.Class;
using LearnSpace.Core.Services;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnSpace.UnitTests
{
	[TestFixture]
	public class ClassServiceTests
	{
		private ClassService classService;
		private Mock<IRepository> mockRepository;

		[SetUp]
		public void Setup()
		{
			mockRepository = new Mock<IRepository>();
			classService = new ClassService(mockRepository.Object);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnTrueIfClassExists()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync(new Course { Id = 1 });

			var result = await classService.ExistsByIdAsync(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnFalseIfClassDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync((Course)null);

			var result = await classService.ExistsByIdAsync(1);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task CreateClassAsync_ShouldAddClassToRepository()
		{
			var model = new CreateClassModel
			{
				TeacherId = Guid.NewGuid().ToString(),
				Name = "Test Class",
				Description = "Test Description",
				GroupCapacity = 30
			};

			await classService.CreateClassAsync(model);

			mockRepository.Verify(r => r.AddAsync(It.Is<Course>(c => c.Name == model.Name && c.Description == model.Description && c.GroupCapacity == model.GroupCapacity)), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task DeleteClassAsync_ShouldRemoveClassAndAssociatedData()
		{
			var studentCourses = new List<StudentCourse>
			{
				new StudentCourse { CourseId = 1, StudentId = Guid.NewGuid() },
				new StudentCourse { CourseId = 1, StudentId = Guid.NewGuid() }
			};

			mockRepository.Setup(r => r.All<StudentCourse>()).Returns(studentCourses.AsQueryable());

			await classService.DeleteClassAsync(1);

			mockRepository.Verify(r => r.DeleteRange(It.Is<IEnumerable<StudentCourse>>(sc => sc.All(s => s.CourseId == 1))), Times.Once);
			mockRepository.Verify(r => r.DeleteAsync<Course>(1), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task GetAllClassesAsync_ShouldReturnFilteredAndSortedClasses()
		{
			var studentId = Guid.NewGuid();
			var student = new Student { Id = studentId };

			mockRepository.Setup(r => r.GetStudentAsync("studentId")).ReturnsAsync(student);
			mockRepository.Setup(r => r.AllReadOnly<Course>()).Returns(new List<Course>
			{
				new Course { Id = 1, Name = "Class A", GroupCapacity = 30, CourseStudents = new List<StudentCourse>(), Teacher = new Teacher { ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" } } },
				new Course { Id = 2, Name = "Class B", GroupCapacity = 25, CourseStudents = new List<StudentCourse>(), Teacher = new Teacher { ApplicationUser = new ApplicationUser { FirstName = "Jane", LastName = "Smith" } } }
			}.AsQueryable());

			var result = await classService.GetAllClassesAsync("studentId");

			Assert.AreEqual(2, result.Classes.Count);
		}

		[Test]
		public async Task GetAllClassesForStudentAsync_ShouldReturnClassesForStudent()
		{
			var student = new Student
			{
				Id = Guid.NewGuid(),
				StudentCourses = new List<StudentCourse>
				{
					new StudentCourse
					{
						Course = new Course
						{
							Id = 1,
							Name = "Class A",
							GroupCapacity = 30,
							Teacher = new Teacher { ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" } }
						}
					}
				}
			};

			mockRepository.Setup(r => r.GetStudentAsync("studentId")).ReturnsAsync(student);

			var result = await classService.GetAllClassesForStudentAsync("studentId");

			Assert.AreEqual(1, result.Classes.Count);
		}

		[Test]
		public async Task GetAllClassesForTeacherAsync_ShouldReturnClassesForTeacher()
		{
			var teacher = new Teacher
			{
				Id = Guid.NewGuid(),
				Courses = new List<Course>
				{
					new Course
					{
						Id = 1,
						Name = "Class A",
						GroupCapacity = 30,
						Teacher = new Teacher { ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" } }
					}
				}
			};

			mockRepository.Setup(r => r.GetTeacherAsync("teacherId")).ReturnsAsync(teacher);

			var result = await classService.GetAllClassesForTeacherAsync("teacherId");

			Assert.AreEqual(1, result.Classes.Count);
		}

		[Test]
		public void GetCreateClassModel_ShouldReturnModelWithTeacherId()
		{
			var teacher = new Teacher { Id = Guid.NewGuid() };

			mockRepository.Setup(r => r.GetTeacherAsync("teacherId")).ReturnsAsync(teacher);

			var result = classService.GetCreateClassModel("teacherId");

			Assert.AreEqual(teacher.Id.ToString().ToLower(), result.TeacherId);
		}

		[Test]
		public async Task JoinClassAsync_ShouldAddStudentToClass()
		{
			var course = new Course { Id = 1, GroupCapacity = 30, CourseStudents = new List<StudentCourse>() };
			var student = new Student { Id = Guid.NewGuid() };

			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync(course);
			mockRepository.Setup(r => r.GetStudentAsync("studentId")).ReturnsAsync(student);

			await classService.JoinClassAsync("studentId", 1);

			mockRepository.Verify(r => r.AddAsync(It.Is<StudentCourse>(sc => sc.StudentId == student.Id && sc.CourseId == 1)), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task LeaveClassAsync_ShouldRemoveStudentFromClass()
		{
			var studentId = Guid.NewGuid();
			var courseId = 1;
			var studentCourse = new StudentCourse { CourseId = courseId, StudentId = studentId };

			var student = new Student
			{
				Id = studentId,
				StudentCourses = new List<StudentCourse> { studentCourse }
			};

			mockRepository.Setup(r => r.GetStudentAsync("studentId")).ReturnsAsync(student);

			// Act
			await classService.LeaveClassAsync("studentId", courseId);

			// Assert
			mockRepository.Verify(r => r.Delete(It.Is<StudentCourse>(sc => sc.CourseId == courseId && sc.StudentId == studentId)), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}
	}
}