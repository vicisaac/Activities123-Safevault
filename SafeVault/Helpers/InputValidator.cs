using System.Text.RegularExpressions;

namespace SafeVault.Helpers
{
    public static class InputValidator
    {
        // Allows only letters, digits, and !@#$%^&*?
        public static bool IsValidInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();
            return Regex.IsMatch(input, @"^[a-zA-Z0-9!@#$%^&*?]+$");
        }

        // Removes potential XSS scripts by stripping HTML tags
        public static string SanitizeForXSS(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            input = input.Trim();
            return Regex.Replace(input, @"<.*?>", string.Empty);
        }
    }
}