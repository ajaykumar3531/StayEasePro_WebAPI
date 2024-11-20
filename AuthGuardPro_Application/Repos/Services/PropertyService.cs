using AuthGuardPro_Application.DTO_s.DTO;
using AuthGuardPro_Application.Repos.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Application.Repos.Contracts;
using StayEasePro_Core.Entities;
using StayEasePro_Domain.DTO_s.Requests;
using StayEasePro_Domain.DTO_s.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static StayEasePro_Domain.Enums.CommonEnums;

namespace StayEasePro_Application.Repos.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWorkService _unitOfWorkService;
        private readonly ILoggerService _logger;

        public PropertyService(IUnitOfWorkService unitOfWorkService, ILoggerService logger)
        {
            _unitOfWorkService = unitOfWorkService;
            _logger = logger;
        }

        public async Task<CreatePropertyResponse> CreateProperty(CreatePropertyRequest request, string UserID)
        {
            try
            {
                CreatePropertyResponse response = new CreatePropertyResponse();

                if (request == null || string.IsNullOrEmpty(UserID))
                {
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                Guid userId;
                if (Guid.TryParse(UserID, out userId))
                {
                    User userData = await _unitOfWorkService.Users.GetByIdAsync(userId);
                    // Continue processing userData as needed
                    if (userData != null)
                    {
                        if (userData.Role == (int)TypeOfUserEnum.Owner)
                        {
                            if (request.PropertyDetails != null && request.PropertyDetails.Any())
                            {
                               var properties = new List<StayEasePro_Core.Entities.Property>();

                                foreach (var propertyData in request.PropertyDetails)
                                {
                                    if (propertyData != null)
                                    {
                                        // Save the corresponding Address
                                        Address address = new Address
                                        {
                                            Street = propertyData.Street,
                                            City = propertyData.City,
                                            State = propertyData.State,
                                            Country = propertyData.Country,
                                            ZipCode = propertyData.ZipCode,
                                            DeleteStatus = false,
                                            CreatedAt = DateTime.Now,
                                            UpdatedAt = DateTime.Now
                                        };

                                        // Add the address to the database (first save the address)
                                        await _unitOfWorkService.Addresses.AddAsync(address);
                                        if (await _unitOfWorkService.Addresses.SaveChangesAsync() > 0)
                                        {
                                            // Create and add the Property with the newly created AddressID
                                            StayEasePro_Core.Entities.Property property = new StayEasePro_Core.Entities.Property
                                            {
                                                OwnerId = Guid.TryParse(UserID.ToUpper(), out var ownerGuid) ? ownerGuid : new Guid(),
                                                PropertyName = propertyData.PropertyName,
                                                TotalRooms = propertyData.TotalRooms,
                                                DeleteStatus = false,
                                                AddressId = address.AddressId,
                                                CreatedAt = DateTime.Now,
                                                UpdatedAt = DateTime.Now  // Link to the newly created Address
                                            };
                                            properties.Add(property);
                                        }
                                        else
                                        {
                                            response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                                            response.StatusCode = StatusCodes.Status404NotFound;
                                        }



                                    }
                                }

                                // Save properties in bulk after all addresses are linked
                                if (properties.Any())
                                {
                                    await _unitOfWorkService.Properties.BulkSave(properties);

                                    // Commit the changes
                                    if (true)
                                    {
                                        response.UserID = UserID;

                                        response.StatusMessage = Constants.MSG_SUCCESS;
                                        response.StatusCode = StatusCodes.Status200OK;
                                    }

                                }
                            }
                        }
                        else
                        {
                            response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                            response.StatusCode = StatusCodes.Status404NotFound;
                        }
                    }
                    else
                    {
                        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                        response.StatusCode = StatusCodes.Status404NotFound;
                    }
                }




                return response;
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                throw;
            }
        }
        public async Task<UpdatePropertyResponse> UpdateProperty(UpdatePropertyRequest request, string UserID)
        {
            try
            {
                UpdatePropertyResponse response = new UpdatePropertyResponse();

                // Check if the request or UserID is invalid
                if (request == null || string.IsNullOrEmpty(UserID))
                {
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                Guid userId;
                if (!Guid.TryParse(UserID, out userId))
                {
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Fetch user data to validate the role
                User userData = await _unitOfWorkService.Users.GetByIdAsync(userId);
                if (userData == null || userData.Role != (int)TypeOfUserEnum.Owner)
                {
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Process each property detail in the request
                if (request.PropertyDetails != null && request.PropertyDetails.Any())
                {
                    foreach (var propertyData in request.PropertyDetails)
                    {
                        if (propertyData != null)
                        {
                            // Find the property by PropertyId
                            if (Guid.TryParse(propertyData.PropertyID, out var propertyId))
                            {
                                var property = await _unitOfWorkService.Properties.GetByIdAsync(propertyId);

                                if (property != null)
                                {
                                    // Update the property details
                                    property.PropertyName = propertyData.PropertyName ?? property.PropertyName;
                                    property.TotalRooms = propertyData.TotalRooms;
                                    property.UpdatedAt = DateTime.Now;

                                    // Update the associated address if it exists
                                    Address address = await _unitOfWorkService.Addresses.GetByIdAsync(property.AddressId);
                                    if (address != null)
                                    {
                                        address.Street = propertyData.Street ?? address.Street;
                                        address.City = propertyData.City ?? address.City;
                                        address.State = propertyData.State ?? address.State;
                                        address.Country = propertyData.Country ?? address.Country;
                                        address.ZipCode = propertyData.ZipCode ?? address.ZipCode;
                                        address.UpdatedAt = DateTime.Now;

                                        // Save the updated address
                                        await _unitOfWorkService.Addresses.UpdateAsync(address);
                                    }
                                    else
                                    {
                                        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                                        response.StatusCode = StatusCodes.Status404NotFound;
                                        return response;
                                    }

                                    // Save the updated property
                                    await _unitOfWorkService.Properties.UpdateAsync(property);
                                }
                                else
                                {
                                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                                    response.StatusCode = StatusCodes.Status404NotFound;
                                    return response;
                                }
                            }
                            else
                            {
                                response.StatusMessage = Constants.MSG_FAILED;
                                response.StatusCode = StatusCodes.Status400BadRequest;
                                return response;
                            }
                        }
                    }

                    // Commit the changes to the database
                    if (await _unitOfWorkService.SaveChangesAsync() > 0)
                    {
                        response.StatusMessage = Constants.MSG_SUCCESS;
                        response.StatusCode = StatusCodes.Status200OK;
                        response.UserID = UserID;
                    }
                    else
                    {
                        response.StatusMessage = Constants.MSG_FAILED;
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                    }
                }
                else
                {
                    response.StatusMessage = Constants.MSG_FAILED;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                }

                return response;
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                throw;
            }



        }
        public async Task<LstPropertiesResponse> GetAllProperties(string UserID)
        {
            try
            {
                LstPropertiesResponse response = new LstPropertiesResponse();

                // Validate UserID
                if (string.IsNullOrEmpty(UserID) || !Guid.TryParse(UserID, out var userId))
                {
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Validate user and role
                var userData = await _unitOfWorkService.Users.GetByIdAsync(userId);
                if (userData == null || userData.Role != (int)TypeOfUserEnum.Owner)
                {
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Fetch properties for the owner
                var properties = await _unitOfWorkService.Properties.GetAllAsync(x => x.OwnerId == userId);
                if (properties == null || !properties.Any())
                {
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Fetch all related addresses in a single query
                var addressIds = properties.Select(p => p.AddressId).Distinct().ToList();
                var addresses = await _unitOfWorkService.Addresses.GetAllAsync(a => addressIds.Contains(a.AddressId));

                // Prepare response
                response.UserID = UserID;
                response.PropertyDetails = properties.Select(property =>
                {
                    var address = addresses.FirstOrDefault(a => a.AddressId == property.AddressId);
                    return new PropertyDetails
                    {
                        PropertyID = property.PropertyId.ToString().ToUpper(),
                        PropertyName = property.PropertyName,
                        TotalRooms = property.TotalRooms,
                        Street = address?.Street,
                        City = address?.City,
                        State = address?.State,
                        Country = address?.Country,
                        ZipCode = address?.ZipCode
                    };
                }).ToList();

                response.StatusMessage = Constants.MSG_SUCCESS;
                response.StatusCode = StatusCodes.Status200OK;

                return response;
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                throw;
            }
        }


    }
}
