using AuthGuardPro_Application.DTO_s.DTO;
using StayEasePro_Domain.DTO_s.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Responses
{
    public class LstPropertiesResponse : StatusDTO
    {
        public string UserID { get; set; } = string.Empty;  
        public List<PropertyDetails> PropertyDetails { get; set; } = new List<PropertyDetails>();
    }
}
