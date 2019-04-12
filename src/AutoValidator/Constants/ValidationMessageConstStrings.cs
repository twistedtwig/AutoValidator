namespace AutoValidator.Constants
{
    public class ValidationMessageConstStrings
    {
        public static string InvalidEmail => "Invalid Email";
        public static string NotNullOrEmpty => "{0} can't be null or empty";
        public static string MinLength => "{1} must be at least {0}";
        public static string MaxLength => "{1} should not be longer than {0}";
    }
}
