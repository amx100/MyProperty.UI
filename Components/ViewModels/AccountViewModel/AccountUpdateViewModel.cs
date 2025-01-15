namespace ViewModels
{
    public class AccountUpdateViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public AccountUpdateDto? Account { get; set; }

        protected async Task UpdateAccount(AccountUpdateDto accountDto)
        {
            try
            {
                var response = await AccountService!.Update(accountDto!.Id!, accountDto);
                HandleCreateOrUpdateResponse(response);
            }
            catch (HttpRequestException)
            {
                MudDialog?.Close(DialogResult.Ok(true));
            }
        }

        private void HandleCreateOrUpdateResponse(GeneralResponseDto response)
        {
            if (response.IsSuccess)
            {
                Snackbar!.Add("Success!",Severity.Success);
                StateHasChanged();
            }
            else
            {
                Snackbar!.Add("Error!", Severity.Error);
            }

            MudDialog?.Close(DialogResult.Ok(true));
        }

        protected void Cancel() => MudDialog?.Cancel();

        protected bool IsDisabled =>
            string.IsNullOrWhiteSpace(Account!.FirstName) ||
            string.IsNullOrWhiteSpace(Account.LastName) ||
            string.IsNullOrWhiteSpace(Account.MobileNumber);
    }
}