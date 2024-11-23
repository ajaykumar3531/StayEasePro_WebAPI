using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;
using StayEasePro_WEBAPP.Services.Contracts;
using StayEasePro_WEBAPP.Data.DTO_s;
using System.Threading.Tasks;
using Blazored.Toast.Services;

namespace StayEasePro_WEBAPP.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiUrlsDto _apiUrls;
        private readonly IToastService _toastService;
        public UserService(IHttpClientService httpClientService, ApiUrlsDto apiUrls, IToastService toastService)
        {
            _httpClientService = httpClientService;
            _apiUrls = apiUrls;  // Injected ApiUrlsDto
            _toastService = toastService;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            try
            {
                CreateUserResponse response = null;

                response = await _httpClientService.PostAsync<CreateUserResponse>(Path.Combine(_apiUrls.Testing, "User/create"), request);

                return response != null ? response : new CreateUserResponse();
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ToastLevel.Error, ex.Message);
                // Handle the error (log, rethrow, etc.)
                throw new Exception("Error occurred while creating user.", ex);
            }
        }
    }
}
