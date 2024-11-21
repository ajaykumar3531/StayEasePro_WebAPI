using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;
using StayEasePro_Domain.DTO_s.Requests;
using StayEasePro_Domain.DTO_s.Responses;

namespace StayEasePro_Application.Repos.Contracts
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task<LoginUserResponse> LoginUser(LoginUserRequest request);
        Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request);
        Task<DeleteResponse> DeleteUser(DeleteRequest request);

        Task<JoinDetailsResponse> JoinUser(JoinDetailsRequest request);
    }
}
