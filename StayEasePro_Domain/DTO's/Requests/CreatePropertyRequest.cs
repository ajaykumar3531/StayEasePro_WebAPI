namespace StayEasePro_Domain.DTO_s.Requests
{
    public class CreatePropertyRequest
    {
        public PropertyDetails PropertyDetails { get; set; } = new PropertyDetails();
        public PropertyAddressDetails AddressDetails { get; set; } = new PropertyAddressDetails();
        public List<RoomDetails> RoomDetails { get; set; } = new List<RoomDetails>();
    }

    public class PropertyDetails
    {
        public string OwnerId { get; set; } = string.Empty;
        public string? PropertyName { get; set; } = string.Empty;
        public long TotalRooms { get; set; }
        public short? NumberOfFloors { get; set; }
        public short? Type { get; set; }
        public short? PropertyType { get; set; }
    }

    public class PropertyAddressDetails
    {

        public string? Street { get; set; } = string.Empty;
        public string? ZipCode { get; set; } = string.Empty;
        public string? StateId { get; set; } = string.Empty;
        public string? CountryId { get; set; } = string.Empty;
        public string? CityId { get; set; } = string.Empty;
        public string? Landmark { get; set; } = string.Empty;

    }


    public class PropertyRoomDetails
    {
        public string RoomNumber { get; set; } = string.Empty;
        public int MaxOccupancy { get; set; } = 0;
        public decimal RentPerMonth { get; set; }
        public bool OccupiedStatus { get; set; }
        public short? FloorNumber { get; set; }
        public string? BlockName { get; set; } = string.Empty;
    }
}
