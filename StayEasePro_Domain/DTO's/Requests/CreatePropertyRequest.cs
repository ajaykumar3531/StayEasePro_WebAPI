namespace StayEasePro_Domain.DTO_s.Requests
{
    public class CreatePropertyRequest
    {
        public List<PropertyDetails> PropertyDetails { get; set; }  = new List<PropertyDetails>();
    }

    public class PropertyDetails
    {

        public string PropertyID { get; set; } = string.Empty;
        public string? PropertyName { get; set; }

        public long TotalRooms { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? ZipCode { get; set; }
    }

   
    public class PropertyRoomDetails
    {
        public string RoomNumber { get; set; } = null!;

        public int MaxOccupancy { get; set; }

        public decimal RentPerMonth { get; set; }

        public bool OccupiedStatus { get; set; }

        public bool DeleteStatus { get; set; }

        public short? FloorNumber { get; set; }

        public string? BlockName { get; set; }
    }
}
