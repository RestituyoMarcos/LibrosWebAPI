namespace LibrosWebAPI.Interfaces
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await _httpClient.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string uri, TValue value)
        {
            return await _httpClient.PostAsJsonAsync(uri, value);
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<TValue>(string uri, TValue value)
        {
            return await _httpClient.PutAsJsonAsync(uri, value);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            return await _httpClient.DeleteAsync(uri);
        }
    }
}
