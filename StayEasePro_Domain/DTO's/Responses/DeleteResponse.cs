using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Application.DTO_s.Responses
{
    public class DeleteResponse : StatusDTO
    {

        public string Email { get; set; } = string.Empty;
      
    }
}
