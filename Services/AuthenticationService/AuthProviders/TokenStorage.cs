namespace AuthProviders
{
    public class TokenStorage(
        ILocalStorageService localStorage)
    {
        public async Task SetTokensAsync(string accessToken, string refreshToken)
        {
            await localStorage.SetItemAsStringAsync("accessToken", accessToken);
            await localStorage.SetItemAsStringAsync("refreshToken", refreshToken);
        }

        public async Task<string> GetAccessToken()
        {
            return await localStorage.GetItemAsync<string>("accessToken");
        }

        public async Task<string> GetRefreshToken()
        {
            return await localStorage.GetItemAsync<string>("refreshToken");
        }

        public async Task RemoveTokens()
        {
            await localStorage.RemoveItemAsync("accessToken");
            await localStorage.RemoveItemAsync("refreshToken");
        }
    }
}