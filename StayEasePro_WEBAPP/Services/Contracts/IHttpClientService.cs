namespace StayEasePro_WEBAPP.Services.Contracts
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string url);
        Task<T> PostAsync<T>(string url, T data);
        Task<T> PutAsync<T>(string url, T data);
        Task DeleteAsync(string url);
        Task<T> SendAsync<T>(HttpRequestMessage request);
    }
}
