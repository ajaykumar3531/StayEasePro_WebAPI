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

        public short Role { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Password { get; set; } = null!;

        public AddressDTO AddressDetails { get; set; } = new AddressDTO();
        public DateTime JoinedDate { get; set; }

        public DateTime ExpectedJoindDate { get; set; }

        public DateOnly DOB { get; set; }

    }
}
