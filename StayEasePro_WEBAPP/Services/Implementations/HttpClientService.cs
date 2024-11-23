using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StayEasePro_WEBAPP.Data.DTO_s;
using System.Text;

namespace StayEasePro_WEBAPP.Services.Contracts
{

    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiUrlsDto _apiUrls;
        public HttpClientService(HttpClient httpClient, IOptions<ApiUrlsDto> apiUrls)
        {
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;

        }

        public string GetApiUrl(string environment)
        {
            return environment switch
            {
                "Testing" => _apiUrls.Testing,
                "Development" => _apiUrls.Development,
                "Production" => _apiUrls.Production,
                _ => _apiUrls.Production // default to production
            };
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                // Handle unsuccessful status codes
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();

                // Check if the content is empty or null
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("The response content is empty.");
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (HttpRequestException e)
            {
                // Log and rethrow if necessary
                throw new Exception("An error occurred while sending the GET request.", e);
            }
            catch (Exception e)
            {
                // Log and rethrow any other exceptions
                throw new Exception("An unexpected error occurred.", e);
            }
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                // Handle unsuccessful status codes
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                // Check if the response content is empty or null
                if (string.IsNullOrEmpty(responseContent))
                {
                    throw new Exception("The response content is empty.");
                }

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                // Log and rethrow if necessary
                throw new Exception("An error occurred while sending the POST request.", ex);
            }
            catch (Exception e)
            {
                // Log and rethrow any other exceptions
                throw new Exception("An unexpected error occurred.", e);
            }
        }

        public async Task<T> PutAsync<T>(string url, object data)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);

                // Handle unsuccessful status codes
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                // Check if the response content is empty or null
                if (string.IsNullOrEmpty(responseContent))
                {
                    throw new Exception("The response content is empty.");
                }

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (HttpRequestException e)
            {
                // Log and rethrow if necessary
                throw new Exception("An error occurred while sending the PUT request.", e);
            }
            catch (Exception e)
            {
                // Log and rethrow any other exceptions
                throw new Exception("An unexpected error occurred.", e);
            }
        }

        public async Task DeleteAsync(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);

                // Handle unsuccessful status codes
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                // Log and rethrow if necessary
                throw new Exception("An error occurred while sending the DELETE request.", e);
            }
            catch (Exception e)
            {
                // Log and rethrow any other exceptions
                throw new Exception("An unexpected error occurred.", e);
            }
        }

        public async Task<T> SendAsync<T>(HttpRequestMessage request)
        {
            try
            {
                var response = await _httpClient.SendAsync(request);

                // Handle unsuccessful status codes
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();

                // Check if the response content is empty or null
                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("The response content is empty.");
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (HttpRequestException e)
            {
                // Log and rethrow if necessary
                throw new Exception("An error occurred while sending the request.", e);
            }
            catch (Exception e)
            {
                // Log and rethrow any other exceptions
                throw new Exception("An unexpected error occurred.", e);
            }
        }
    }
}
