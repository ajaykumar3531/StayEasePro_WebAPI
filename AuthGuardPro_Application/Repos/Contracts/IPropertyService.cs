using AuthGuardPro_Application.DTO_s.DTO;
using StayEasePro_Domain.DTO_s.Requests;
using StayEasePro_Domain.DTO_s.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Application.Repos.Contracts
{
    public interface IPropertyService
    {
        Task<CreatePropertyResponse> CreateProperty(CreatePropertyRequest request, string UserID);
        Task<UpdatePropertyResponse> UpdateProperty(UpdatePropertyRequest request, string UserID);
        Task<LstPropertiesResponse> GetAllProperties(string UserID);
        Task<DeletePropertyResponse> DeleteProperties(DeletePropertyRequest request, string UserID);
    }
}
