namespace Services
{
    public class AuthenticationService(IApiService apiService, HttpClient client, ILocalStorageService localStorage) : IAuthenticationService
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<AuthenticationDto> Login(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Post($"{ApiEndpoints.AccountController}/login", loginDto);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<AuthenticationDto>(responseStream, _options,
                    cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<GeneralResponseDto> Register(RegistrationDto registrationDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response =
                    await apiService.Post($"{ApiEndpoints.AccountController}/registration", registrationDto);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options,
                    cancellationToken);
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