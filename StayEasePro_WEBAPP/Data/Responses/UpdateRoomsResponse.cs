using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Responses
{
    public class UpdateRoomsResponse : StatusDTO
    {
        public string UserID { get; set; }
        public List<string> UpdatedRoomIds { get; set; } = new List<string>();
    }
}
