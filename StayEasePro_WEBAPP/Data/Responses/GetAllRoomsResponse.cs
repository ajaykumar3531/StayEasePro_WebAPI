using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Responses
{
    public class GetAllRoomsResponse : StatusDTO
    {
        public List<RoomDetails> RoomDetails { get; set; } = new List<RoomDetails>();
    }

    public class RoomDetails
    {
        public string RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal RentPerMonth { get; set; }
        public bool OccupiedStatus { get; set; }
    }

}
