using LearnSpace.Core.Models.Grade;
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
	public class GradeServiceTests
	{
		private GradeService gradeService;
		private Mock<IRepository> mockRepository;

		[SetUp]
		public void Setup()
		{
			mockRepository = new Mock<IRepository>();
			gradeService = new GradeService(mockRepository.Object);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnTrueIfGradeExists()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Grade>(1)).ReturnsAsync(new Grade { Id = 1 });

			var result = await gradeService.ExistsByIdAsync(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnFalseIfGradeDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Grade>(1)).ReturnsAsync((Grade)null);

			var result = await gradeService.ExistsByIdAsync(1);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task CreateGradeAsync_ShouldAddGradeToRepository()
		{
			var model = new CreateGradeViewModel
			{
				CourseId = 1,
				StudentId = Guid.NewGuid().ToString(),
				Score = 95,
				Description = "Excellent work"
			};

			await gradeService.CreateGradeAsync(model);

			mockRepository.Verify(r => r.AddAsync(It.Is<Grade>(g => g.CourseId == model.CourseId && g.Score == model.Score && g.Description == model.Description)), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task DeleteGradeAsync_ShouldRemoveGradeFromRepository()
		{
			var grade = new Grade { Id = 1, CourseId = 2 };

			mockRepository.Setup(r => r.GetByIdAsync<Grade>(1)).ReturnsAsync(grade);

			var result = await gradeService.DeleteGradeAsync(1);

			mockRepository.Verify(r => r.DeleteAsync<Grade>(1), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
			Assert.AreEqual(2, result);
		}

		[Test]
		public async Task GetAddGradeModelAsync_ShouldReturnModelWithCourseAndStudentDetails()
		{
			var student = new Student
			{
				Id = Guid.NewGuid(),
				ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" }
			};

			var course = new Course
			{
				Id = 1,
				Name = "Math"
			};

			mockRepository.Setup(r => r.GetByIdAsync<Student>(It.IsAny<Guid>())).ReturnsAsync(student);
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync(course);

			var result = await gradeService.GetAddGradeModelAsync(1, student.Id.ToString());

			Assert.AreEqual("Math", result.CourseName);
			Assert.AreEqual("John Doe", result.StudentName);
		}

		[Test]
		public async Task GetAllGradesAsync_ShouldReturnAllGradesForStudent()
		{
			var studentId = Guid.NewGuid();
			var courseId = 1;

			var grades = new List<Grade>
			{
			new Grade { Id = 1, Score = 95, StudentId = studentId },
			new Grade { Id = 2, Score = 85, StudentId = studentId }
			};

			var course = new Course
			{
				Id = courseId,
				Name = "Math",
				Grades = grades
			};

			var student = new Student
			{
				Id = studentId,
				StudentCourses = new List<StudentCourse>
				{
					new StudentCourse { Course = course }
				}
			};

			mockRepository.Setup(r => r.GetStudentAsync("studentId")).ReturnsAsync(student);

			// Act
			var result = await gradeService.GetAllGradesAsync("studentId");

			// Assert
			Assert.AreEqual(1, result.Count); // Expect one course
			Assert.AreEqual("Math", result[0].Name);
			Assert.AreEqual(2, result[0].Grades.Count);
		}

		[Test]
		public async Task GetGradeInfoAsync_ShouldReturnGradeDetails()
		{
			var grade = new Grade
			{
				Id = 1,
				Score = 95,
				Description = "Excellent work",
				Course = new Course
				{
					Name = "Math",
					Teacher = new Teacher
					{
						ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" }
					}
				}
			};

			mockRepository.Setup(r => r.GetByIdAsync<Grade>(1)).ReturnsAsync(grade);

			var result = await gradeService.GetGradeInfoAsync(1);

			Assert.AreEqual("Math", result.CourseName);
			Assert.AreEqual(95, result.Score);
			Assert.AreEqual("John Doe", result.Teacher);
		}

		[Test]
		public async Task ClassExistsByIdAsync_ShouldReturnTrueIfClassExists()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync(new Course { Id = 1 });

			var result = await gradeService.ClassExistsByIdAsync(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ClassExistsByIdAsync_ShouldReturnFalseIfClassDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync((Course)null);

			var result = await gradeService.ClassExistsByIdAsync(1);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task UserExistsByIdAsync_ShouldReturnTrueIfUserExists()
		{
			var student = new Student { Id = Guid.NewGuid() };

			mockRepository.Setup(r => r.GetByIdAsync<Student>(It.IsAny<Guid>())).ReturnsAsync(student);

			var result = await gradeService.UserExistsByIdAsync(student.Id.ToString());

			Assert.IsTrue(result);
		}

		[Test]
		public async Task UserExistsByIdAsync_ShouldReturnFalseIfUserDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Student>(It.IsAny<Guid>())).ReturnsAsync((Student)null);

			var result = await gradeService.UserExistsByIdAsync(Guid.NewGuid().ToString());

			Assert.IsFalse(result);
		}
	}
}
