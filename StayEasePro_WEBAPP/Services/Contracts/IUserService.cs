using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;

namespace StayEasePro_WEBAPP.Services.Contracts
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
    }
}
