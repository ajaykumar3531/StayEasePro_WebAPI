using AuthGuardPro_Application.DTO_s.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Application.CommonRepos.Contracts
{
    public interface IAuthService
    {
        Task<string> TokenGeneration(TokenRequest request);

        Task AuthorizeUser();
    }
}
