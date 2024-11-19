using AuthGuardPro_Application.DTO_s.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Application.DTO_s.Responses
{
    public class CreateUserResponse : StatusDTO
    {
        public Guid UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        
        public DateTime? DateCreated { get; set; }
    }
}
