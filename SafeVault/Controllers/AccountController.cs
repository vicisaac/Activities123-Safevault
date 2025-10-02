// SafeVault/Controllers/AccountController.cs
// Improved with the help of Copilot to analyze and fix security issues.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SafeVault.Models;
using SafeVault.DataAccess;
using SafeVault.Helpers;

public class AccountController : Controller
{
    private readonly UserRepository _repo;

    public AccountController(IConfiguration config)
    {
        var connStr = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connStr))
        {
            throw new ArgumentException("Connection string 'DefaultConnection' is missing or empty.");
        }

        _repo = new UserRepository(connStr);
    }


    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(RegisterModel model)
    {
        if (!ModelState.IsValid ||
            !InputValidator.IsValidInput(model.Username) ||
            !InputValidator.IsValidInput(model.Password))
        {
            TempData["ErrorMessage"] = "Registration failed. Invalid input detected.";
            return RedirectToAction("Index", "Home");
        }

        var user = new User
        {
            Username = InputValidator.SanitizeForXSS(model.Username),
            Email = InputValidator.SanitizeForXSS(model.Email),
            PasswordHash = PasswordHelper.HashPassword(model.Password),
            Role = InputValidator.SanitizeForXSS(model.Role)
        };

        _repo.Register(user);
        TempData["SuccessMessage"] = "Registration successful!";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(LoginModel model)
    {
        if (!ModelState.IsValid || !InputValidator.IsValidInput(model.Username))
        {
            TempData["ErrorMessage"] = "Login failed. Invalid input detected.";
            return RedirectToAction("Index", "Home");
        }

        var user = _repo.GetUser(InputValidator.SanitizeForXSS(model.Username));
        if (user == null || !PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
        {
            TempData["ErrorMessage"] = "Invalid username or password.";
            return RedirectToAction("Index", "Home");
        }

        TempData["SuccessMessage"] = $"Welcome back, {InputValidator.SanitizeForXSS(user.Username)}!";
        return RedirectToAction("Index", "Home");
    }
}