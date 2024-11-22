using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Responses
{
    public class DeleteRoomsResponse : StatusDTO
    {
        public List<string> DeletedRoomIds { get; set; } = new List<string>();
    }

}
