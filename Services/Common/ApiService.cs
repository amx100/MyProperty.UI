namespace Services
{
    public class ApiService(HttpClient httpClient, ILocalStorageService localStorage) : IApiService
    {
        private async Task AddAuthorizationHeader()
        {
            try
            {
                var accessToken = await localStorage!.GetItemAsync<string>("accessToken");
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                else
                {
                    Console.WriteLine("Warning: No access token found in local storage");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding authorization header: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            try
            {
                Console.WriteLine($"Sending DELETE request to: {url}");
                
                // Dodajemo auth header pre svakog zahteva
                await AddAuthorizationHeader();
                
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                var response = await httpClient.SendAsync(request);
                
                // Logujemo response za debugging
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Delete response status: {response.StatusCode}");
                Console.WriteLine($"Delete response content: {content}");

                // Proveravamo specifične status kodove
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized request - check authentication token");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Resource not found");
                }
                else if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Delete request failed with status: {response.StatusCode}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error in Delete: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in Delete: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            try
            {
                await AddAuthorizationHeader();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                return await httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get request: {ex.Message}");
                throw;
            }
        }
       
        public async Task<HttpResponseMessage> Put(string url, object obj)
        {
            try
            {
                await AddAuthorizationHeader();
                var json = new StringContent(
                    JsonSerializer.Serialize(obj),
                    Encoding.UTF8,
                    Application.Json);
                return await httpClient.PutAsync(url + "/", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Put request: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> Post(string url, object obj)
        {
            try
            {
                await AddAuthorizationHeader();
                var json = new StringContent(
                    JsonSerializer.Serialize(obj),
                    Encoding.UTF8,
                    Application.Json);
                return await httpClient.PostAsync(url, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Post request: {ex.Message}");
                throw;
            }
        }
    }
}