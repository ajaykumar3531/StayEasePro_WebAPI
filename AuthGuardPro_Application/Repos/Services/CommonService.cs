using AuthGuardPro_Application.DTO_s.DTO;
using Azure;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Application.Repos.Contracts;
using StayEasePro_Core.Entities;
using StayEasePro_Domain.DTO_s.Requests;
using StayEasePro_Domain.DTO_s.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Application.Repos.Services
{
    public class CommonService : ICommonService
    {
        private readonly IUnitOfWorkService _unitOfWorkService;
        private readonly ILoggerService _logger;

        public CommonService(IUnitOfWorkService unitOfWorkService, ILoggerService logger)
        {
            _unitOfWorkService = unitOfWorkService;
            _logger = logger;
        }

        public async Task<ListOfCitiesResponse> GetAllCities(string stateID)
        {
            try
            {
                ListOfCitiesResponse response = new ListOfCitiesResponse();

                if (stateID == null || string.IsNullOrEmpty(stateID))
                {
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                var cities = await _unitOfWorkService.Cities.GetAllAsync(x => x.StateId == Guid.Parse(stateID));

                if (cities != null && cities.Any())
                {

                    response.CityDetails = new List<CityDetails>();
                    foreach (var city in cities)
                    {
                        if (city != null)
                        {
                            response.CityDetails.Add(new CityDetails
                            {
                                CityId = city.CityId.ToString(),
                                CityName = city.CityName,
                                StateId = city.StateId.ToString()
                            });
                        }
                    }
                    response.StatusMessage = Constants.MSG_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                }

                return response;
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                throw ex;
            }
        }

        public async Task<ListOfStatesResponse> GetAllStates(string countryID)
        {
            try
            {
                ListOfStatesResponse response = new ListOfStatesResponse();

                if (countryID == null || string.IsNullOrEmpty(countryID))
                {
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                var states = await _unitOfWorkService.State.GetAllAsync(x => x.CountryId == Guid.Parse(countryID));

                if (states != null && states.Any())
                {

                    response.StateDetails = new List<StateDetails>();
                    foreach (var state in states)
                    {
                        if (state != null)
                        {
                            response.StateDetails.Add(new StateDetails
                            {
                                CountryId = state.CountryId.ToString(),
                                StateId = state.StateId.ToString(),
                                StateName = state.StateName,
                            });
                        }
                    }
                    response.StatusMessage = Constants.MSG_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                }

                return response;
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                throw ex;
            }
        }
    }
}
