using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Requests
{
    public class ListOfStatesResponse : StatusDTO
    {

        public List<StateDetails> StateDetails { get; set; } = new List<StateDetails>();

    }

    public class StateDetails{
        public string StateId { get; set; } = string.Empty;

        public string StateName { get; set; } = string.Empty;

        public string CountryId { get; set; } = string.Empty;

    }
}
