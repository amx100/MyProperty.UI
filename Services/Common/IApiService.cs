namespace Services
{
    public interface IApiService
    {
        public Task<HttpResponseMessage> Delete(string url);

        public Task<HttpResponseMessage> Get(string url);

        public Task<HttpResponseMessage> Post(string url, object obj);

        public Task<HttpResponseMessage> Put(string url, object obj);
    }
}