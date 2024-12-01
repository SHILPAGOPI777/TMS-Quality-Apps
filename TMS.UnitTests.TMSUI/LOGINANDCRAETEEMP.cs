using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using TMS.Application.Employees.Commands.CreateEmployee;
using TMS.Persistence.Identity;
using TMS.WebUI.Areas.Identity.Pages.Account;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using TMS.Application.Common.Interfaces;
using TMS.Domain.Entities;

namespace TMS.Tests.Areas.Identity.Pages.Account
{
    [TestFixture]
    public class AdminLoginAndCreateEmployeeTests
    {
        private Mock<SignInManager<AppUser>> _signInManagerMock;
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<ILogger<LoginModel>> _loggerMock;
        private LoginModel _loginModel;
        private TestApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            // Initialize mock UserManager and SignInManager
            _userManagerMock = new Mock<UserManager<AppUser>>(
                new Mock<IUserStore<AppUser>>().Object,
                null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<AppUser>>(
                _userManagerMock.Object,
                new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
                new Mock<Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<AppUser>>().Object,
                null, null, null, null);

            _loggerMock = new Mock<ILogger<LoginModel>>();

            _loginModel = new LoginModel(_signInManagerMock.Object, _loggerMock.Object, _userManagerMock.Object)
            {
                Input = new LoginModel.InputModel
                {
                    Email = "admin@example.com",
                    Password = "Admin@123",
                    RememberMe = true
                }
            };

            // Set up in-memory database context with Employees DbSet
            var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new TestApplicationDbContext(options);
        }

        [Test]
        public async Task AdminLoginAndCreateEmployee_ShouldLogInAndCreateEmployeeSuccessfully()
        {
            // Arrange - Step 1: Mock successful login
            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync("admin@example.com", "Admin@123", true, false))
                .ReturnsAsync(SignInResult.Success);

            var returnUrl = "~/";

            // Act - Step 1: Attempt to log in
            var loginResult = await _loginModel.OnPostAsync(returnUrl);
            var redirectResult = loginResult as LocalRedirectResult;

            // Assert - Step 1: Verify login success and redirection to the expected URL
            Assert.IsNotNull(redirectResult, "Expected a redirect result after login.");
            Assert.AreEqual(returnUrl, redirectResult.Url, "Expected redirection to the dashboard URL after login.");

            // Arrange - Step 2: Set up employee creation command
            var createEmployeeCommand = new CreateEmployeeCommand { FullName = "New Employee" };
            var handler = new CreateEmployeeCommand.CreateEmployeeCommandHandler(_context);

            // Act - Step 2: Create employee
            var createResult = await handler.Handle(createEmployeeCommand, CancellationToken.None);

            // Assert - Step 2: Verify employee creation in the database
            var entity = await _context.Employees.FindAsync(createResult);
            Assert.IsNotNull(entity, "Employee entity should have been persisted but was not found.");
            Assert.AreEqual("New Employee", entity.FullName, "The FullName of the employee does not match the expected value.");

            TestContext.WriteLine("Test passed: Admin login and employee creation succeeded.");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
