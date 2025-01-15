namespace ViewModels;

public class RegistrationViewModel : ComponentBaseViewModel
{
    protected RegistrationDto Registration = new();
    
    protected string ValidationMessage { get; set; } = "Required";
    protected bool ShowAuthError { get; set; }
    
    bool showPassword;
    
    protected InputType PasswordInput = InputType.Password;
    
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

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

    protected async Task ExecuteRegistration()
    {
        ShowAuthError = false;
        var result = await AuthenticationService!.Register(Registration);
        Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.TopLeft;
        if (result.IsSuccess)
        {
            Snackbar!.Add("Registration successful!", Severity.Success);
            NavigationManager!.NavigateTo("/");
        }
        else
        {
            Snackbar!.Add("Registration failed.", Severity.Error);
        }
    }

    protected void NavigateToLogin()
    {
        NavigationManager!.NavigateTo("/");
    }
}