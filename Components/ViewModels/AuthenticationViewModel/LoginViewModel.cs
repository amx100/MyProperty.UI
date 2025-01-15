namespace ViewModels;

public class LoginViewModel : ComponentBaseViewModel
{
    protected LoginDto Login = new();
    protected bool ShowAuthError { get; set; }
    protected string? Error { get; set; }
    
    protected bool showPassword;
    protected InputType PasswordInput = InputType.Password;
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        // var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        // var user = authState.User;
        // // if (user.Identity!.IsAuthenticated)
        // // {
        // //     navigationManager.NavigateTo("/dashboard");
        // // }
    }

    protected async Task ExecuteLogin()
    {
        ShowAuthError = false;
        var response = await AuthenticationService!.Login(Login);
        if (!response.IsSuccessful)
        {
            Error = response.ErrorMessage;
            ShowAuthError = true;
        }
        else
        {
            await InsertDataIntoLocalStorage(response);
            NavigationManager!.NavigateTo("/dashboard");
        }
    }

    private async Task InsertDataIntoLocalStorage(AuthenticationDto response)
    {
        await LocalStorage!.SetItemAsync("accountId", response!.AccountId!.ToString()!);
        await LocalStorage.SetItemAsStringAsync("accessToken", response.AccessToken!);
        await LocalStorage!.SetItemAsync<string>("accountFirstName", response!.AccountFirstName!);
    }

    protected void ShowHidePassword()
    {
        if (showPassword)
        {
            showPassword = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            showPassword = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    protected void OpenForgotPasswordPage()
    {
        NavigationManager!.NavigateTo("forgot-password");
    }

    protected void Registration()
    {
        NavigationManager!.NavigateTo("registration");
    }
}