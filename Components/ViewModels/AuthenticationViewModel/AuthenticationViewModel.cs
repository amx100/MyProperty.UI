namespace ViewModels
{
    public class AuthenticationViewModel(
        HttpClient client,
        TokenAuthenticationStateProvider authStateProvider,
        ISnackbar snackbar,
        ILocalStorageService localStorage,
        NavigationManager navigationManager,
        IAuthenticationService authenticationService) : IAuthenticationViewModel
    {
        private ChangePasswordDto _changePassword = new();

        public string SuccessMessage { get; set; } = string.Empty;
        public bool IsSuccess;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool ShowAuthError { get; set; }


        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("accessToken");
            await localStorage.RemoveItemAsync("accountId");
            await localStorage.RemoveItemAsync("refreshToken");
            client.DefaultRequestHeaders.Authorization = null;
            navigationManager.NavigateTo("/", true);
        }

        public ChangePasswordDto ChangePassword
        {
            get => _changePassword;
            set => _changePassword = value;
        }
    }
}