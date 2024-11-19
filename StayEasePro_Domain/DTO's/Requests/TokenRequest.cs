using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Application.DTO_s.Requests
{
    public class TokenRequest
    {
        public string UserID { get; set; }=string.Empty;
        public string Email {  get; set; }=string.Empty;    
         
    }
}
