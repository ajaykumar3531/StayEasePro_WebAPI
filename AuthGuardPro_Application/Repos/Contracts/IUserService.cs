using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;

namespace AuthGuardPro_Application.Repos.Contracts
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task<LoginUserResponse> LoginUser(LoginUserRequest request);
        Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request);
        Task<DeleteResponse> DeleteUser(DeleteRequest request);
    }
}
