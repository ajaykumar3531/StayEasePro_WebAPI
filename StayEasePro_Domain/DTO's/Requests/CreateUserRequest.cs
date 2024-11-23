using StayEasePro_Domain.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Application.DTO_s.Requests
{
    public class CreateUserRequest
    {

        public short Role { get; set; } = 0;

        public string? FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public AddressDTO AddressDetails { get; set; } = new AddressDTO();
        public DateTime? JoinedDate { get; set; } = null;

        public DateTime? ExpectedJoindDate { get; set; } = null;

        public DateOnly? DOB { get; set; } = null;

    }
}
