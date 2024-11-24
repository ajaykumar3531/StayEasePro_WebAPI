using StayEasePro_Domain.DTO_s.Requests;
using StayEasePro_Domain.DTO_s.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Application.Repos.Contracts
{
    public interface ICommonService
    {
        Task<ListOfStatesResponse> GetAllStates(string countryID);
        Task<ListOfCitiesResponse> GetAllCities(string countryID);
    }
}
