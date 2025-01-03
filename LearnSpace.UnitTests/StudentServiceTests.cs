using LearnSpace.Core.Services;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Moq;

namespace LearnSpace.UnitTests
{
	[TestFixture]
	public class StudentServiceTests
	{
		private StudentService studentService;
		private Mock<IRepository> mockRepository;

		[SetUp]
		public void Setup()
		{
			mockRepository = new Mock<IRepository>();
			studentService = new StudentService(mockRepository.Object);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnTrueIfStudentExists()
		{
			var userId = Guid.NewGuid().ToString();
			var user = new ApplicationUser { Id = Guid.Parse(userId), Student = new Student() };

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync(user);

			var result = await studentService.ExistsByIdAsync(userId);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnFalseIfStudentDoesNotExist()
		{
			var userId = Guid.NewGuid().ToString();
			var user = new ApplicationUser { Id = Guid.Parse(userId), Student = null };

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync(user);

			var result = await studentService.ExistsByIdAsync(userId);

			Assert.IsFalse(result);
		}

	//	[Test]
	//	public async Task GetStudentDashboardInformationAsync_ShouldReturnCorrectDashboardModel()
	//	{
	//		// Arrange
	//		var studentId = Guid.NewGuid();
	//		var student = new Student
	//		{
	//			Id = studentId,
	//			ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" },
	//			StudentCourses = new List<StudentCourse>
	//	{
	//		new StudentCourse
	//		{
	//			Course = new Course
	//			{
	//				Assignments = new List<Assignment> { new Assignment(), new Assignment() },
	//				Grades = new List<Grade>
	//				{
	//					new Grade { StudentId = studentId, Score = 90, DateGraded = DateTime.UtcNow },
	//					new Grade { StudentId = studentId, Score = 80, DateGraded = DateTime.UtcNow.AddDays(-1) }
	//				}
	//			}
	//		}
	//	}
	//		};

	//		var grades = new List<Grade>
	//{
	//	new Grade { StudentId = studentId, Score = 90, DateGraded = DateTime.UtcNow },
	//	new Grade { StudentId = studentId, Score = 80, DateGraded = DateTime.UtcNow.AddDays(-1) }
	//};

	//		mockRepository.Setup(r => r.GetStudentAsync(studentId.ToString())).ReturnsAsync(student);
	//		mockRepository.Setup(r => r.AllReadOnly<Grade>())
	//			.Returns(grades.AsQueryable().ToAsyncEnumerable());

	//		// Act
	//		var result = await studentService.GetStudentDashboardInformationAsync(studentId.ToString());

	//		// Assert
	//		Assert.AreEqual("John Doe", result.FullName);
	//		Assert.AreEqual(2, result.GradeCount);
	//		Assert.AreEqual(1, result.ClassCount);
	//		Assert.AreEqual(2, result.AssignmentCount);
	//		Assert.AreEqual(85, result.Success);
	//		Assert.AreEqual(2, result.ChartData.Count);
	//	}
		
	}
}
