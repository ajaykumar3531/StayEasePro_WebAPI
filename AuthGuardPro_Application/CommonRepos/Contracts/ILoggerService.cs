using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Application.CommonRepos.Contracts
{
    public interface ILoggerService
    {
        Task LocalLogs(Exception ex);

    }
}
