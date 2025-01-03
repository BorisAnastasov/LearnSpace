using LearnSpace.Core.Services;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Moq;


namespace LearnSpace.UnitTests
{
	[TestFixture]
	public class TeacherServiceTests
	{
		private TeacherService teacherService;
		private Mock<IRepository> mockRepository;

		[SetUp]
		public void Setup()
		{
			mockRepository = new Mock<IRepository>();
			teacherService = new TeacherService(mockRepository.Object);
		}

		[Test]
		public async Task ClassExistsByIdAsync_ShouldReturnTrueIfClassExists()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync(new Course { Id = 1 });

			var result = await teacherService.ClassExistsByIdAsync(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ClassExistsByIdAsync_ShouldReturnFalseIfClassDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync((Course)null);

			var result = await teacherService.ClassExistsByIdAsync(1);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnTrueIfTeacherExists()
		{
			var userId = Guid.NewGuid().ToString();
			var user = new ApplicationUser { Id = Guid.Parse(userId), Student = new Student() };

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync(user);

			var result = await teacherService.ExistsByIdAsync(userId);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnFalseIfTeacherDoesNotExist()
		{
			var userId = Guid.NewGuid().ToString();
			var user = new ApplicationUser { Id = Guid.Parse(userId), Student = null };

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync(user);

			var result = await teacherService.ExistsByIdAsync(userId);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetGradeBookByClassAsync_ShouldReturnCorrectGradeBook()
		{
			var teacherId = "teacherId";
			var classId = 1;
			var teacher = new Teacher
			{
				Id = Guid.NewGuid(),
				Courses = new List<Course>
				{
					new Course
					{
						Id = classId,
						CourseStudents = new List<StudentCourse>
						{
							new StudentCourse
							{
								Student = new Student
								{
									Id = Guid.NewGuid(),
									ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" },
									Grades = new List<Grade>
									{
										new Grade { Id = 1, Score = 90, CourseId = classId },
										new Grade { Id = 2, Score = 85, CourseId = classId }
									}
								}
							}
						}
					}
				}
			};

			mockRepository.Setup(r => r.GetTeacherAsync(teacherId)).ReturnsAsync(teacher);
			mockRepository.Setup(r => r.GetByIdAsync<Course>(classId)).ReturnsAsync(teacher.Courses.First());

			var result = await teacherService.GetGradeBookByClassAsync(teacherId, classId);

			Assert.AreEqual(classId, result.ClassId);
			Assert.AreEqual(1, result.List.Count);
			Assert.AreEqual("John Doe", result.List.First().StudentName);
			Assert.AreEqual(2, result.List.First().Grades.Count);
		}

		[Test]
		public async Task GetTeacherDashboardInformationAsync_ShouldReturnCorrectDashboardModel()
		{
			var teacherId = "teacherId";
			var teacher = new Teacher
			{
				ApplicationUser = new ApplicationUser { FirstName = "Jane", LastName = "Smith" },
				Courses = new List<Course>
				{
					new Course
					{
						CourseStudents = new List<StudentCourse>
						{
							new StudentCourse(),
							new StudentCourse()
						},
						Assignments = new List<Assignment>
						{
							new Assignment
							{
								Submissions = new List<Submission> { new Submission(), new Submission() }
							},
							new Assignment()
						}
					}
				}
			};

			mockRepository.Setup(r => r.GetTeacherAsync(teacherId)).ReturnsAsync(teacher);

			var result = await teacherService.GetTeacherDashboardInformationAsync(teacherId);

			Assert.AreEqual("Jane Smith", result.FullName);
			Assert.AreEqual(2, result.TotalStudentsEnrolled);
			Assert.AreEqual(2, result.AssignmentCount);
			Assert.AreEqual(2, result.WaitingSubmissions);
		}
	}
}
