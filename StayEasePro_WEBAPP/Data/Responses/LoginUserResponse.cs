using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Application.DTO_s.Responses
{
    public class LoginUserResponse : StatusDTO
    {
        
        public string Email { get; set; } = string.Empty;

        public string JWTToken { get;set; } = string.Empty; 
    }
}
