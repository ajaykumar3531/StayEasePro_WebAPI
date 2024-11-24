using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Responses
{
    public class ListOfCitiesResponse  : StatusDTO
    {
        public List<CityDetails> CityDetails { get; set; } = new List<CityDetails>();

    }

    public class CityDetails
    {
        public string CityId { get; set; } = string.Empty;

        public string CityName { get; set; } = string.Empty;

        public string StateId { get; set; } = string.Empty;
    }
}
