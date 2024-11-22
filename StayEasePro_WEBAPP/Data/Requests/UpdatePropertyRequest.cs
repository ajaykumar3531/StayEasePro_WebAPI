using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Requests
{
    public class UpdatePropertyRequest
    {
        public List<PropertyDetails> PropertyDetails { get; set; } = new List<PropertyDetails>();
    }
}
