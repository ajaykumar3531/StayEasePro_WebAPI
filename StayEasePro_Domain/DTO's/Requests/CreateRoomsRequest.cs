using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Requests
{
    public class CreateRoomsRequest
    {
        public string PropertyId { get; set; } = null!;
        public List<RoomDetails> RoomDetails { get; set; } = new List<RoomDetails>();
    }

    public class RoomDetails
    {
        public string RoomNumber { get; set; } = null!;
        public int MaxOccupancy { get; set; }
        public decimal RentPerMonth { get; set; }
        public bool OccupiedStatus { get; set; }
    }
}
