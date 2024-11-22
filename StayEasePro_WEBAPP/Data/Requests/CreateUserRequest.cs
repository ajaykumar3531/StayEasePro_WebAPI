using StayEasePro_Domain.DTO_s.DTO;
using static StayEasePro_WebApplication.Data.Enums.CommonEnums;

namespace AuthGuardPro_Application.DTO_s.Requests
{
    public class CreateUserRequest
    {

        public short Role { get; set; } = (int)TypeOfUserEnum.Tenet;

        public string? FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Password { get; set; } = null!;

        public AddressDTO AddressDetails { get; set; } = null;
        public DateTime? JoinedDate { get; set; } = null;

        public DateTime? ExpectedJoindDate { get; set; } = null;

        public DateOnly? DOB { get; set; } = null;

    }
}
