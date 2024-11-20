using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Requests
{
    public class CreatePropertyRequest
    {
        public List<PropertyDetails> PropertyDetails { get; set; }  = new List<PropertyDetails>();
    }

    public class PropertyDetails
    {
       
        public string PropertyID { get; set; }
        public string? PropertyName { get; set; }

        public long TotalRooms { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? ZipCode { get; set; }
    }
}
