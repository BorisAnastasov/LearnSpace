using LearnSpace.Core.Models.Assignment;
using LearnSpace.Core.Services;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Moq;

namespace LearnSpace.UnitTests
{
	[TestFixture]
	public class AssignmentServiceTests
	{
		private AssignmentService assignmentService;
		private Mock<IRepository> mockRepository;

		[SetUp]
		public void Setup()
		{
			mockRepository = new Mock<IRepository>();
			assignmentService = new AssignmentService(mockRepository.Object);
		}

		[Test]
		public async Task CreateAssignment_ShouldAddAssignmentToRepository()
		{
			var model = new CreateAssignmentFormModel
			{
				Title = "Test Assignment",
				Description = "Test Description",
				DueDate = "2024-12-31",
				ClassId = 1
			};

			await assignmentService.CreateAssignment(model);

			mockRepository.Verify(r => r.AddAsync(It.Is<Assignment>(a => a.Title == model.Title && a.Description == model.Description && a.CourseId == model.ClassId)), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public void CreateAssignment_ShouldThrowExceptionForInvalidDateFormat()
		{
			var model = new CreateAssignmentFormModel
			{
				Title = "Test Assignment",
				Description = "Test Description",
				DueDate = "31-12-2024",
				ClassId = 1
			};

			Assert.ThrowsAsync<ArgumentException>(async () => await assignmentService.CreateAssignment(model));
		}

		[Test]
		public async Task DeleteAssignment_ShouldDeleteAssignmentFromRepository()
		{
			var assignment = new Assignment { Id = 1, CourseId = 2 };

			mockRepository.Setup(r => r.GetByIdAsync<Assignment>(1)).ReturnsAsync(assignment);

			var result = await assignmentService.DeleteAssignment(1);

			mockRepository.Verify(r => r.DeleteAsync<Assignment>(1), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
			Assert.AreEqual(2, result);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnTrueIfAssignmentExists()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Assignment>(1)).ReturnsAsync(new Assignment { Id = 1 });

			var result = await assignmentService.ExistsByIdAsync(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnFalseIfAssignmentDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Assignment>(1)).ReturnsAsync((Assignment)null);

			var result = await assignmentService.ExistsByIdAsync(1);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetAllAssignmentsAsync_ShouldReturnAllAssignmentsForStudent()
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
							Assignments = new List<Assignment>
							{
								new Assignment { Id = 1, Title = "Assignment 1", DueDate = DateTime.Parse("2024-12-31"), Submissions = new List<Submission>() },
								new Assignment { Id = 2, Title = "Assignment 2", DueDate = DateTime.Parse("2024-11-30"), Submissions = new List<Submission>() }
							}
						}
					}
				}
			};

			mockRepository.Setup(r => r.GetStudentAsync(It.IsAny<string>())).ReturnsAsync(student);

			var result = await assignmentService.GetAllAssignmentsAsync("studentId");

			Assert.AreEqual(2, result.Assignments.Count);
		}

		[Test]
		public async Task GetAllAssignmentsByClassForStudentAsync_ShouldReturnAssignmentsForClass()
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
							Assignments = new List<Assignment>
							{
								new Assignment { Id = 1, Title = "Assignment 1", DueDate = DateTime.Parse("2024-12-31"), Submissions = new List<Submission>() },
								new Assignment { Id = 2, Title = "Assignment 2", DueDate = DateTime.Parse("2024-11-30"), Submissions = new List<Submission>() }
							}
						}
					}
				}
			};

			mockRepository.Setup(r => r.GetStudentAsync(It.IsAny<string>())).ReturnsAsync(student);
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync(student.StudentCourses.First().Course);

			var result = await assignmentService.GetAllAssignmentsByClassForStudentAsync("studentId", 1);

			Assert.AreEqual(2, result.Assignments.Count);
			Assert.AreEqual("Assignment 1", result.Assignments[0].Title);
		}

		[Test]
		public async Task GetAllAssignmentsByClassForTeacherAsync_ShouldReturnAssignmentsForClass()
		{
			var teacher = new Teacher
			{
				Id = Guid.NewGuid(),
				Courses = new List<Course>
				{
					new Course
					{
						Id = 1,
						Assignments = new List<Assignment>
						{
							new Assignment { Id = 1, Title = "Assignment 1", DueDate = DateTime.Parse("2024-12-31") },
							new Assignment { Id = 2, Title = "Assignment 2", DueDate = DateTime.Parse("2024-11-30") }
						}
					}
				}
			};

			mockRepository.Setup(r => r.GetTeacherAsync(It.IsAny<string>())).ReturnsAsync(teacher);
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync(teacher.Courses.First());

			var result = await assignmentService.GetAllAssignmentsByClassForTeacherAsync("teacherId", 1);

			Assert.AreEqual(2, result.Assignments.Count);
		}

		[Test]
		public async Task GetAssignmentInfoAsync_ShouldReturnAssignmentDetailsForStudent()
		{
			var student = new Student
			{
				Id = Guid.NewGuid()
			};

			var assignment = new Assignment
			{
				Id = 1,
				Title = "Assignment 1",
				Description = "Description",
				DueDate = DateTime.Parse("2024-12-31"),
				Course = new Course
				{
					Name = "Course 1",
					Teacher = new Teacher
					{
						ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" }
					}
				},
				Submissions = new List<Submission>()
			};

			mockRepository.Setup(r => r.GetByIdAsync<Assignment>(1)).ReturnsAsync(assignment);
			mockRepository.Setup(r => r.GetStudentAsync(It.IsAny<string>())).ReturnsAsync(student);

			var result = await assignmentService.GetAssignmentInfoAsync("studentId", 1);

			Assert.AreEqual("Assignment 1", result.Title);
			Assert.AreEqual("Course 1", result.ClassName);
		}

		[Test]
		public async Task GetAssignmentInfoForTeacherAsync_ShouldReturnAssignmentDetails()
		{
			var assignment = new Assignment
			{
				Id = 1,
				Title = "Assignment 1",
				Description = "Description",
				DueDate = DateTime.Parse("2024-12-31"),
				Course = new Course
				{
					Name = "Course 1"
				}
			};

			mockRepository.Setup(r => r.GetByIdAsync<Assignment>(1)).ReturnsAsync(assignment);

			var result = await assignmentService.GetAssignmentInfoForTeacherAsync(1);

			Assert.AreEqual("Assignment 1", result.Title);
			Assert.AreEqual("Course 1", result.ClassName);
		}

		[Test]
		public async Task ClassExistsByIdAsync_ShouldReturnTrueIfClassExists()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync(new Course { Id = 1 });

			var result = await assignmentService.ClassExistsByIdAsync(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ClassExistsByIdAsync_ShouldReturnFalseIfClassDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Course>(1)).ReturnsAsync((Course)null);

			var result = await assignmentService.ClassExistsByIdAsync(1);

			Assert.IsFalse(result);
		}
	}
}

