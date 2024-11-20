using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Requests
{
    public class DeleteRoomsRequest
    {
        public List<string> RoomIds { get; set; } = new List<string>();
    }

}
