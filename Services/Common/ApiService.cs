namespace Services
{
    public class ApiService(HttpClient httpClient, ILocalStorageService localStorage) : IApiService
    {
        private async Task AddAuthorizationHeader()
        {
            var accessToken = await localStorage!.GetItemAsync<string>("accessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            await AddAuthorizationHeader();
            return await httpClient.SendAsync(request);
        }
       
        public async Task<HttpResponseMessage> Put(string url, object obj)
        {
            var json = new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                Application.Json);
            return await httpClient.PutAsync(url + "/", json);
        }

        public async Task<HttpResponseMessage> Post(string url, object obj)
        {
            var json = new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                Application.Json);
            return await httpClient.PostAsync(url, json);
        }
    }
}