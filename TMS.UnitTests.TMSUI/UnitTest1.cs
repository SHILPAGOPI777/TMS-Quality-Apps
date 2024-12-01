using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using TMS.Persistence.Identity;
using TMS.WebUI.Areas.Identity.Pages.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TMS.Tests.Areas.Identity.Pages.Account
{
    [TestFixture]
    public class LoginModelTests
    {
        private Mock<SignInManager<AppUser>> _signInManagerMock;
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<ILogger<LoginModel>> _loggerMock;
        private LoginModel _loginModel;

        [SetUp]
        public void SetUp()
        {
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
                    Email = "admin@test.ru",
                    Password = "1_Aadmin@test.ru",
                    RememberMe = true
                }
            };
        }

        [Test]
        public async Task OnPostAsync_ValidCredentials_ShouldRedirectToDashboard()
        {
            // Arrange
            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync("testuser@example.com", "Test@12", true, false))
                .ReturnsAsync(SignInResult.Success);

            var expectedDashboardUrl = "localhost";

            // Act
            var result = await _loginModel.OnPostAsync(expectedDashboardUrl);

            // Assert
            var redirectResult = result as LocalRedirectResult;
            Assert.IsNotNull(redirectResult, "Expected a redirect result.");
            Assert.AreEqual(expectedDashboardUrl, redirectResult.Url, "Expected redirection to the dashboard URL.");
        }
    }
}
