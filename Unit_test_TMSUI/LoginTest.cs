using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TMS.WebUI.Areas.Identity.Pages.Account;
using TMS.Persistence.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace TMS.WebUI.Tests
{
    [TestFixture]
    public class LoginModelTests
    {
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<SignInManager<AppUser>> _signInManagerMock;
        private Mock<ILogger<LoginModel>> _loggerMock;
        private LoginModel _loginModel;

        [SetUp]
        public void SetUp()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(),
                null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<AppUser>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null, null, null, null);

            _loggerMock = new Mock<ILogger<LoginModel>>();

            _loginModel = new LoginModel(_signInManagerMock.Object, _loggerMock.Object, _userManagerMock.Object);
        }

        [Test]
        public async Task OnGetAsync_WithValidReturnUrl_SetsReturnUrlAndExternalLogins()
        {
            // Arrange
            var returnUrl = "/some-url";
            _signInManagerMock.Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>
                {
                    new AuthenticationScheme("Google", "Google", typeof(IAuthenticationHandler))
                });

            // Act
            await _loginModel.OnGetAsync(returnUrl);

            // Assert
            Assert.AreEqual(returnUrl, _loginModel.ReturnUrl);
            Assert.AreEqual(1, _loginModel.ExternalLogins.Count);
            Assert.AreEqual("Google", _loginModel.ExternalLogins[0].Name);
        }

        [Test]
        public async Task OnPostAsync_ValidCredentials_RedirectsToReturnUrl()
        {
            // Arrange
            var returnUrl = "/home";
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "test@example.com",
                Password = "Password123",
                RememberMe = false
            };

            _signInManagerMock.Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _loginModel.OnPostAsync(returnUrl);

            // Assert
            var redirectResult = result as LocalRedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(returnUrl, redirectResult.Url);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public async Task OnPostAsync_InvalidLoginAttempt_ReturnsPageWithError()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "wrong@example.com",
                Password = "wrongpassword",
                RememberMe = false
            };

            _signInManagerMock.Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsTrue(_loginModel.ModelState.ContainsKey(string.Empty));
        }

        [Test]
        public async Task OnPostAsync_AccountLockedOut_RedirectsToLockoutPage()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "test@example.com",
                Password = "Password123",
                RememberMe = false
            };

            _signInManagerMock.Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.LockedOut);

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            var redirectResult = result as RedirectToPageResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("./Lockout", redirectResult.PageName);
        }
    }
}
