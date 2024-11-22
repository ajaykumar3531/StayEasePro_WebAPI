using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Domain.DTO_s.Responses
{
    public class DeletePropertyRequest
    {
        public List<string> PropertyIDs { get; set; } = new List<string>();
    }

}
