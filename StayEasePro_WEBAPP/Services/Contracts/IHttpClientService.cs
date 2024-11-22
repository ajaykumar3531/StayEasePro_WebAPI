namespace StayEasePro_WEBAPP.Services.Contracts
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string url);
        Task<T> PostAsync<T>(string url, object data);
        Task<T> PutAsync<T>(string url, object data);
        Task DeleteAsync(string url);
        Task<T> SendAsync<T>(HttpRequestMessage request);
    }
}
