namespace AuthProviders
{
    public class TokenStorage(
        ILocalStorageService localStorage)
    {
        private const string ACCESS_TOKEN_KEY = "accessToken";
        private const string REFRESH_TOKEN_KEY = "refreshToken";
        private const string ACCOUNT_ID_KEY = "accountId";

        public async Task SetTokensAsync(string accessToken, string refreshToken)
        {
            await localStorage.SetItemAsStringAsync(ACCESS_TOKEN_KEY, accessToken);
            await localStorage.SetItemAsStringAsync(REFRESH_TOKEN_KEY, refreshToken);
        }

        public async Task<string> GetAccessToken()
        {
            return await localStorage.GetItemAsync<string>(ACCESS_TOKEN_KEY);
        }

        public async Task<string> GetRefreshToken()
        {
            return await localStorage.GetItemAsync<string>(REFRESH_TOKEN_KEY);
        }

        public async Task<string> GetAccountId()
        {
            return await localStorage.GetItemAsync<string>(ACCOUNT_ID_KEY);
        }

        public async Task SetAccountId(string accountId)
        {
            await localStorage.SetItemAsStringAsync(ACCOUNT_ID_KEY, accountId);
        }

        public async Task RemoveTokens()
        {
            await localStorage.RemoveItemAsync(ACCESS_TOKEN_KEY);
            await localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
            await localStorage.RemoveItemAsync(ACCOUNT_ID_KEY);
        }
    }
}