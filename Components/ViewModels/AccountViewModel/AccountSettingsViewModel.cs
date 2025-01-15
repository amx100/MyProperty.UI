using Components.Accounts;

namespace ViewModels
{
    public class AccountSettingsViewModel : ComponentBaseViewModel
    {
        public AccountDto Account { get; set; } = new();

        protected bool Loading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadAccountInfo();
            Loading = false;
        }

        public async Task OpenAccountDialog(AccountDto accountDto)
        {
            var options = new DialogOptions()
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium
            };

            var accountForUpdate = accountDto.Adapt<AccountUpdateDto>();

            var parameters = new DialogParameters { ["Account"] = accountForUpdate };
            const string text = "Account Update";
            var dialog = await DialogService!.ShowAsync<AccountUpdateComponent>(text, parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await LoadAccountInfo();
                StateHasChanged();
            }
        }

        private async Task LoadAccountInfo()
        {
            var accountId = await LocalStorage!.GetItemAsync<string>("accountId");
            try
            {
                Account = await AccountService!.GetAccountById(accountId);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}