using System.Net.Http.Headers;

namespace ViewModels
{
    public class AuthenticationViewModel(
        HttpClient client,
        TokenAuthenticationStateProvider authStateProvider,
        ISnackbar snackbar,
        TokenStorage tokenStorage,
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
            await tokenStorage.RemoveTokens();
            client.DefaultRequestHeaders.Authorization = null;
            navigationManager.NavigateTo("/", true);
        }

        public ChangePasswordDto ChangePassword
        {
            get => _changePassword;
            set => _changePassword = value;
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            try
            {
                var result = await authenticationService.Login(loginDto);
                
                if (result?.IsSuccessful == true)
                {
                    client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    
                    authStateProvider.StateChanged();
                    navigationManager.NavigateTo("/");
                    return true;
                }
                
                ErrorMessage = result?.ErrorMessage ?? "Login failed";
                ShowAuthError = true;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                ErrorMessage = "An error occurred during login";
                ShowAuthError = true;
                return false;
            }
        }
    }
}