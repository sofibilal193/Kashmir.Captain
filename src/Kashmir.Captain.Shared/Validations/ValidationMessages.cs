
namespace Kashmir.Captain.Shared.Validations
{
    public static class ValidationMessages
    {
        public const string UserNotFound = "User not found.";
        public const string InvalidCredentials = "Invalid username or password.";
        public const string AccessDenied = "You do not have permission to perform this action.";
        public const string OperationSuccessful = "Operation completed successfully.";
        public const string OperationFailed = "Operation failed. Please try again.";
        public const string InternalServerError = "An unexpected error occurred. Please contact support.";
        public const string EmailAlreadyExists = "Email address is already registered.";
        public const string PasswordResetSuccess = "Password has been reset successfully.";
        public const string TokenExpired = "The token has expired.";
        public const string Confirm_Email_Address = $"Confirm Email {GlobalConstants.ProjectName}";
        public const string User_Registered_Success = $"User registered successfully. Please check your email to confirm your account.";
    }

}