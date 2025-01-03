using LearnSpace.Core.Services;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Identity;
using Moq;


namespace LearnSpace.UnitTests
{
	[TestFixture]
	public class AdminServiceTests
	{
		private AdminService adminService;
		private Mock<IRepository> mockRepository;
		private Mock<UserManager<ApplicationUser>> mockUserManager;
		private Mock<RoleManager<IdentityRole<Guid>>> mockRoleManager;

		[SetUp]
		public void Setup()
		{
			mockRepository = new Mock<IRepository>();

			var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
			mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

			var roleStoreMock = new Mock<IRoleStore<IdentityRole<Guid>>>();
			mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(roleStoreMock.Object, null, null, null, null);

			adminService = new AdminService(mockUserManager.Object, mockRepository.Object, mockRoleManager.Object);
		}

		[Test]
		public async Task AddRoleAsync_ShouldAddRoleToUser()
		{
			var userId = Guid.NewGuid().ToString();
			var role = "Administrator";
			var user = new ApplicationUser { Id = Guid.Parse(userId) };

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync(user);

			await adminService.AddRoleAsync(userId, role);

			mockUserManager.Verify(um => um.AddToRoleAsync(user, role), Times.Once);
		}

		[Test]
		public async Task DeleteRoleAsync_ShouldRemoveRoleFromUser()
		{
			var userId = Guid.NewGuid().ToString();
			var role = "Admin";
			var user = new ApplicationUser { Id = Guid.Parse(userId) };

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync(user);

			await adminService.DeleteRoleAsync(userId, role);

			mockUserManager.Verify(um => um.RemoveFromRoleAsync(user, role), Times.Once);
		}

		[Test]
		public async Task DeleteUserAsync_ShouldDeleteUserAndAssociatedData()
		{
			var userId = Guid.NewGuid().ToString();
			var user = new ApplicationUser { Id = Guid.Parse(userId), Student = new Student() };
			var student = new Student { Id = Guid.Parse(userId), StudentCourses = new List<StudentCourse>() };

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync(user);
			mockRepository.Setup(r => r.GetStudentAsync(userId)).ReturnsAsync(student);

			await adminService.DeleteUserAsync(userId);

			mockRepository.Verify(r => r.DeleteRange<StudentCourse>(student.StudentCourses), Times.Once);
			mockRepository.Verify(r => r.DeleteAsync<Student>(student.Id), Times.Once);
			mockRepository.Verify(r => r.DeleteAsync<ApplicationUser>(Guid.Parse(userId)), Times.Once);
			mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task GetAllUsersAsync_ShouldReturnAllUsersWithRoles()
		{
			var users = new List<ApplicationUser>
			{
				new ApplicationUser { Id = Guid.NewGuid(), UserName = "User1", FirstName = "First1", LastName = "Last1" },
				new ApplicationUser { Id = Guid.NewGuid(), UserName = "User2", FirstName = "First2", LastName = "Last2" }
			};

			mockRepository.Setup(r => r.AllReadOnly<ApplicationUser>()).Returns(users.AsQueryable());

			foreach (var user in users)
			{
				mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(user.Id)).ReturnsAsync(user);
				mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Role1" });
			}

			var result = await adminService.GetAllUsersAsync();

			Assert.AreEqual(users.Count, result.Count);
			foreach (var user in result)
			{
				Assert.IsTrue(user.Roles.Contains("Role1"));
			}
		}

		[Test]
		public async Task RoleExistsByNameAsync_ShouldReturnTrueIfRoleExists()
		{
			var roleName = "Admin";

			mockRoleManager.Setup(rm => rm.RoleExistsAsync(roleName)).ReturnsAsync(true);

			var result = await adminService.RoleExistsByNameAsync(roleName);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task RoleExistsByNameAsync_ShouldThrowExceptionForInvalidRole()
		{
			var roleName = "";

			Assert.ThrowsAsync<ArgumentException>(async () => await adminService.RoleExistsByNameAsync(roleName));
		}

		[Test]
		public async Task UserExistsByIdAsync_ShouldReturnTrueIfUserExists()
		{
			var userId = Guid.NewGuid().ToString();
			var user = new ApplicationUser { Id = Guid.Parse(userId) };

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync(user);

			var result = await adminService.UserExistsByIdAsync(userId);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task UserExistsByIdAsync_ShouldReturnFalseIfUserDoesNotExist()
		{
			var userId = Guid.NewGuid().ToString();

			mockRepository.Setup(r => r.GetByIdAsync<ApplicationUser>(Guid.Parse(userId))).ReturnsAsync((ApplicationUser)null);

			var result = await adminService.UserExistsByIdAsync(userId);

			Assert.IsFalse(result);
		}
	}
}
