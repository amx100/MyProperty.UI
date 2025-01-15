namespace ViewModels
{
    public interface IAuthenticationViewModel
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public ChangePasswordDto ChangePassword { get; set; }
        public bool ShowAuthError { get; set; }
        public Task Logout();
    }
}