using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Requests
{
    public class UpdateRoomsRequest
    {
        public string PropertyId { get; set; } = null!;
        public List<UpdateRoomDetails> RoomDetails { get; set; } = new List<UpdateRoomDetails>();
    }

    public class UpdateRoomDetails
    {
        public string RoomId { get; set; } = null!;
        public string? RoomNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal RentPerMonth { get; set; }
        public bool OccupiedStatus { get; set; }
    }
}
