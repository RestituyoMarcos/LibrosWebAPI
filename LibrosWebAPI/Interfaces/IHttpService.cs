namespace LibrosWebAPI.Interfaces
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> GetAsync(string uri);
        Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string uri, TValue value);
        Task<HttpResponseMessage> PutAsJsonAsync<TValue>(string uri, TValue value);
        Task<HttpResponseMessage> DeleteAsync(string uri);
    }
}
