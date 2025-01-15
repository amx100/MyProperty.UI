namespace Services
{
    public class AccountService(IApiService apiService) : IAccountService
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<GeneralResponseDto> Delete(string accountId)
        {
            try
            {
                var response = await apiService.Delete($"{ApiEndpoints.AccountController}/{accountId}");
                return response.IsSuccessStatusCode
                    ? new GeneralResponseDto { IsSuccess = true }
                    : new GeneralResponseDto { IsSuccess = false };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new GeneralResponseDto { IsSuccess = false };
            }
        }

        public async Task<AccountDto> GetAccountById(string accountId)
        {
            try
            {
                var response = await apiService.Get($"{ApiEndpoints.AccountController}/details/{accountId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var res = await JsonSerializer.DeserializeAsync<AccountDto>(responseStream, _options);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ObservableCollection<AccountDto>> GetAllAccounts()
        {
            try
            {
                var response = await apiService.Get($"{ApiEndpoints.AccountController}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<AccountDto>>(responseStream, _options);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<GeneralResponseDto> Update(string accountId, AccountUpdateDto account)
        {
            try
            {
                var response = await apiService.Put($"{ApiEndpoints.AccountController}/update/{accountId}", account);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }
    }
}