namespace AutoValidator.Constants
{
    public class ValidationMessageConstStrings
    {
        public static string InvalidEmail => "Invalid Email";
        public static string StringNotNullOrEmpty => "{0} can't be null or empty";
        public static string StringMinLength => "{1} must be at least {0}";
        public static string StringMaxLength => "{1} should not be longer than {0}";

        public static string IntMinValue => "{1} should be at least {0}";
    }
}
