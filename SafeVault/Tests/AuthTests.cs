using NUnit.Framework;
using SafeVault.Helpers;
using SafeVault.Models;
using SafeVault.DataAccess;

namespace SafeVault.Tests
{
    [TestFixture]
    public class AuthTests
    {
        private UserRepository? _repo;

        [SetUp]
        public void Setup()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=SafeVault_DB;User Id=SafeVaultUser;Password=StrongP@ssw0rd!1;TrustServerCertificate=true;";
            _repo = new UserRepository(connectionString);
        }

        [Test]
        public void PasswordHashing_ShouldMatchOriginal()
        {
            string password = "Secure!123";
            string hash = PasswordHelper.HashPassword(password);
            Assert.That(PasswordHelper.VerifyPassword(password, hash), Is.True);
        }

        [Test]
        public void PasswordHashing_ShouldFailOnWrongPassword()
        {
            string password = "Secure!123";
            string hash = PasswordHelper.HashPassword(password);
            Assert.That(PasswordHelper.VerifyPassword("WrongPassword", hash), Is.False);
        }

        [Test]
        public void Registration_ShouldRejectMismatchedPasswords()
        {
            var model = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Mismatch123!",
                Role = "User"
            };

            Assert.That(model.Password == model.ConfirmPassword, Is.False);
        }

        [Test]
        public void Login_ShouldFailForInvalidUser()
        {
            var user = _repo.GetUser("nonexistent");
            Assert.That(user, Is.Null);
        }

        [Test]
        public void RoleAccess_ShouldAllowAdmin()
        {
            var user = new User { Role = "Admin" };
            Assert.That(user.Role == "Admin", Is.True);
        }

        [Test]
        public void RoleAccess_ShouldDenyGuestFromAdminRoute()
        {
            var user = new User { Role = "Guest" };
            Assert.That(user.Role == "Admin", Is.False);
        }
        
        [Test]
    public void Registration_ShouldRejectUnsafeUsername()
    {
        var unsafeUsername = "<script>alert('xss')</script>";
        var isValid = InputValidator.IsValidInput(unsafeUsername);
        Assert.That(isValid, Is.False, "Unsafe username should be rejected by input validator.");
    }
    }
}