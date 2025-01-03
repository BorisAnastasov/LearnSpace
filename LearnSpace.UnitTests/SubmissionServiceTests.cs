using LearnSpace.Core.Services;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Http;
using Moq;

namespace LearnSpace.UnitTests
{
	[TestFixture]
	public class SubmissionServiceTests
	{
		private SubmissionService submissionService;
		private Mock<IRepository> mockRepository;

		[SetUp]
		public void Setup()
		{
			mockRepository = new Mock<IRepository>();
			submissionService = new SubmissionService(mockRepository.Object);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnTrueIfSubmissionExists()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Submission>(1)).ReturnsAsync(new Submission { Id = 1 });

			var result = await submissionService.ExistsByIdAsync(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task ExistsByIdAsync_ShouldReturnFalseIfSubmissionDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Submission>(1)).ReturnsAsync((Submission)null);

			var result = await submissionService.ExistsByIdAsync(1);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task CreateSubmissionAsync_ShouldAddNewSubmissionIfNotExists()
		{
			var userId = "studentId";
			var assignmentId = 1;
			var fileMock = new Mock<IFormFile>();
			var content = "Test File Content";
			var fileName = "test.pdf";
			var memoryStream = new MemoryStream();
			var writer = new StreamWriter(memoryStream);

			writer.Write(content);
			writer.Flush();
			memoryStream.Position = 0;

			fileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
			fileMock.Setup(f => f.FileName).Returns(fileName);
			fileMock.Setup(f => f.ContentType).Returns("application/pdf");
			fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Callback<Stream, CancellationToken>((stream, _) => memoryStream.CopyTo(stream)).Returns(Task.CompletedTask);

			var student = new Student { Id = Guid.NewGuid(), Submissions = new List<Submission>() };

			mockRepository.Setup(r => r.GetStudentAsync(userId)).ReturnsAsync(student);

			await submissionService.CreateSubmissionAsync(userId, assignmentId, fileMock.Object);

			mockRepository.Verify(r => r.AddAsync(It.Is<Submission>(s => s.AssignmentId == assignmentId && s.FileName == fileName)), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task CreateSubmissionAsync_ShouldUpdateExistingSubmission()
		{
			var userId = "studentId";
			var assignmentId = 1;
			var fileMock = new Mock<IFormFile>();
			var content = "Updated File Content";
			var fileName = "updated.pdf";
			var memoryStream = new MemoryStream();
			var writer = new StreamWriter(memoryStream);

			writer.Write(content);
			writer.Flush();
			memoryStream.Position = 0;

			fileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
			fileMock.Setup(f => f.FileName).Returns(fileName);
			fileMock.Setup(f => f.ContentType).Returns("application/pdf");
			fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Callback<Stream, CancellationToken>((stream, _) => memoryStream.CopyTo(stream)).Returns(Task.CompletedTask);

			var existingSubmission = new Submission { Id = 1, AssignmentId = assignmentId, FileName = "old.pdf" };
			var student = new Student { Id = Guid.NewGuid(), Submissions = new List<Submission> { existingSubmission } };

			mockRepository.Setup(r => r.GetStudentAsync(userId)).ReturnsAsync(student);

			await submissionService.CreateSubmissionAsync(userId, assignmentId, fileMock.Object);

			Assert.AreEqual(fileName, existingSubmission.FileName);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task GetAllSubmissionsForAssignmentAsync_ShouldReturnSubmissionsForAssignment()
		{
			var assignmentId = 1;
			var assignment = new Assignment
			{
				Id = assignmentId,
				Title = "Test Assignment",
				Submissions = new List<Submission>
				{
					new Submission
					{
						Id = 1,
						FileName = "test.pdf",
						SubmittedOn = DateTime.UtcNow,
						Student = new Student
						{
							ApplicationUser = new ApplicationUser { FirstName = "John", LastName = "Doe" }
						}
					}
				}
			};

			mockRepository.Setup(r => r.GetByIdAsync<Assignment>(assignmentId)).ReturnsAsync(assignment);

			var result = await submissionService.GetAllSubmissionsForAssignmentAsync(assignmentId);

			Assert.AreEqual(1, result.Submissions.Count);
			Assert.AreEqual("John Doe", result.Submissions.First().StudentName);
		}

		[Test]
		public async Task GetFileBySubmissionIdAsync_ShouldReturnCorrectFile()
		{
			var submissionId = 1;
			var submission = new Submission
			{
				Id = submissionId,
				FileName = "test.pdf",
				FileType = "application/pdf",
				FileContent = new byte[] { 0x1, 0x2, 0x3 }
			};

			mockRepository.Setup(r => r.GetByIdAsync<Submission>(submissionId)).ReturnsAsync(submission);

			var result = await submissionService.GetFileBySubmissionIdAsync(submissionId);

			Assert.AreEqual("test.pdf", result.FileName);
			Assert.AreEqual("application/pdf", result.FileType);
			CollectionAssert.AreEqual(new byte[] { 0x1, 0x2, 0x3 }, result.FileContent);
		}

		[Test]
		public async Task DeleteSubmissionIdAsync_ShouldDeleteSubmission()
		{
			var submissionId = 1;

			await submissionService.DeleteSubmissionIdAsync(submissionId);

			mockRepository.Verify(r => r.DeleteAsync<Submission>(submissionId), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public void ContainsOnlyAllowedFileTypeAsync_ShouldReturnTrueForValidFile()
		{
			var fileMock = new Mock<IFormFile>();
			fileMock.Setup(f => f.FileName).Returns("test.pdf");

			var result = submissionService.ContainsOnlyAllowedFileType(fileMock.Object);

			Assert.IsTrue(result);
		}

		[Test]
		public void ContainsOnlyAllowedFileTypeAsync_ShouldReturnFalseForInvalidFile()
		{
			var fileMock = new Mock<IFormFile>();
			fileMock.Setup(f => f.FileName).Returns("test.exe");

			var result = submissionService.ContainsOnlyAllowedFileType(fileMock.Object);

			Assert.IsFalse(result);
		}

		[Test]
		public void SizeIsNotTooBig_ShouldReturnTrueForValidSize()
		{
			var fileMock = new Mock<IFormFile>();
			fileMock.Setup(f => f.Length).Returns(4 * 1024 * 1024); // 4 MB

			var result = submissionService.SizeIsNotTooBig(fileMock.Object);

			Assert.IsTrue(result);
		}

		[Test]
		public void SizeIsNotTooBig_ShouldReturnFalseForTooLargeFile()
		{
			var fileMock = new Mock<IFormFile>();
			fileMock.Setup(f => f.Length).Returns(6 * 1024 * 1024); // 6 MB

			var result = submissionService.SizeIsNotTooBig(fileMock.Object);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task AssignmentExistsByIdAsync_ShouldReturnTrueIfAssignmentExists()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Assignment>(1)).ReturnsAsync(new Assignment { Id = 1 });

			var result = await submissionService.AssignmentExistsByIdAsync(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task AssignmentExistsByIdAsync_ShouldReturnFalseIfAssignmentDoesNotExist()
		{
			mockRepository.Setup(r => r.GetByIdAsync<Assignment>(1)).ReturnsAsync((Assignment)null);

			var result = await submissionService.AssignmentExistsByIdAsync(1);

			Assert.IsFalse(result);
		}
	}
}
