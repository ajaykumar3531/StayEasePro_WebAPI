using StayEasePro_Domain.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Requests
{
    public class JoinDetailsRequest
    {
        public string UserId { get; set; } = string.Empty;

        public string RoomId { get; set; } = string.Empty;

        public string PropertyID { get; set; } = string.Empty;

        public AddressDTO AddressDetails { get; set; } = new AddressDTO();
    }

}
