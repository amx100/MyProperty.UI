using AuthProviders;

namespace Services
{
    public class AuthenticationService(
        IApiService apiService, 
        HttpClient client, 
        TokenStorage tokenStorage) : IAuthenticationService
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<AuthenticationDto> Login(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Post($"{ApiEndpoints.AccountController}/login", loginDto);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                
                Console.WriteLine($"Login response: {content}"); // Debug log

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Login failed with status code: {response.StatusCode}");
                    return new AuthenticationDto 
                    { 
                        IsSuccessful = false,
                        ErrorMessage = $"Login failed: {response.StatusCode}" 
                    };
                }

                var result = JsonSerializer.Deserialize<AuthenticationDto>(content, _options);
                
                if (result?.IsSuccessful == true)
                {
                    // Set the authorization header
                    client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", result.AccessToken);

                    // Store tokens using TokenStorage
                    await tokenStorage.SetTokensAsync(result.AccessToken, result.RefreshToken);
                    await tokenStorage.SetAccountId(result.AccountId);

                    Console.WriteLine($"Login successful for account: {result.AccountId}");
                }
                else
                {
                    Console.WriteLine($"Login failed: {result?.ErrorMessage}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during login: {ex.Message}");
                return new AuthenticationDto 
                { 
                    IsSuccessful = false,
                    ErrorMessage = $"Login error: {ex.Message}" 
                };
            }
        }

        public async Task<GeneralResponseDto> Register(RegistrationDto registrationDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Post($"{ApiEndpoints.AccountController}/registration", registrationDto);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                
                if (!response.IsSuccessStatusCode)
                {
                    return new GeneralResponseDto 
                    { 
                        IsSuccess = false,
                        Message = $"Registration failed: {response.StatusCode}" 
                    };
                }

                return JsonSerializer.Deserialize<GeneralResponseDto>(content, _options) ?? 
                    new GeneralResponseDto { IsSuccess = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return new GeneralResponseDto 
                { 
                    IsSuccess = false,
                    Message = $"Registration error: {ex.Message}" 
                };
            }
        }
    }
}