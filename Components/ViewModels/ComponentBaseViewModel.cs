global using AuthProviders;
global using Blazored.LocalStorage;
global using Mapster;
global using Microsoft.AspNetCore.Components;
global using MudBlazor;
global using Services;
global using Dto;
using Services.ReservationService;

namespace ViewModels
{
    public class ComponentBaseViewModel : ComponentBase
    {


        [Inject] public IPropertyService? PropertyService { get; set; }

        [Inject] public IPropertyImageService? PropertyImageService { get; set; }

        [Inject] public IReservationService? ReservationService { get; set; }

        
        [Inject] public IAccountService? AccountService { get; set; }

        [Inject] public IAuthenticationService? AuthenticationService { get; set; }

        [Inject] public NavigationManager? NavigationManager { get; set; }

        [Inject] public TokenAuthenticationStateProvider? StateProvider { get; set; }

        [Inject] public IDialogService? DialogService { get; set; }

        [Inject] public ISnackbar? Snackbar { get; set; }
        
        [Inject] public IAuthenticationViewModel? AuthenticationViewModel { get; set; }

        [Inject] public ILocalStorageService? LocalStorage { get; set; }

        protected const string InfoFormat = "{first_item}-{last_item} of {all_items}";
    }
}