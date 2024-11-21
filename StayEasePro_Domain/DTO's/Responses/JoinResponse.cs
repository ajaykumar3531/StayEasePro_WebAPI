using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Responses
{
    public class JoinDetailsResponse : StatusDTO
    {
        public string UserId { get; set; } = string.Empty;

        public string AddressId { get; set; } = string.Empty;

        public string TenantId { get; set; } = string.Empty;
    }

}
