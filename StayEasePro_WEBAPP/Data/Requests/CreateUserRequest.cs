using StayEasePro_Domain.DTO_s.DTO;
using static StayEasePro_WebApplication.Data.Enums.CommonEnums;

namespace AuthGuardPro_Application.DTO_s.Requests
{
    public class CreateUserRequest
    {

        public short Role { get; set; } = (int)TypeOfUserEnum.Tenet;

        public string? FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        

    }
}
