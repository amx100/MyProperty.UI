namespace ViewModels;

public class RegistrationViewModel : ComponentBaseViewModel
{
    protected RegistrationDto Registration = new();
    
    protected string ValidationMessage { get; set; } = "Required";
    protected bool ShowAuthError { get; set; }
    
    bool showPassword;
    
    protected InputType PasswordInput = InputType.Password;
    
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override void OnInitialized()
    {
        // Set default role for registration
        Registration.Role = "User";
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

    protected async Task ExecuteRegistration()
    {
        ShowAuthError = false;
        
        try 
        {
            var result = await AuthenticationService!.Register(Registration);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.TopLeft;
            
            if (result.IsSuccess)
            {
                Snackbar!.Add("Registration successful! Please login.", Severity.Success);
                NavigationManager!.NavigateTo("/");
            }
            else
            {
                ShowAuthError = true;
                Snackbar!.Add(result.Message ?? "Registration failed.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            ShowAuthError = true;
            Snackbar!.Add($"Registration error: {ex.Message}", Severity.Error);
        }
    }

    protected void NavigateToLogin()
    {
        NavigationManager!.NavigateTo("/");
    }
}