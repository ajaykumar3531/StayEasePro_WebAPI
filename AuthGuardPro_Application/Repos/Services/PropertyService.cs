using AuthGuardPro_Application.DTO_s.DTO;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Application.Repos.Contracts;
using StayEasePro_Core.Entities;
using StayEasePro_Domain.DTO_s.Requests;
using StayEasePro_Domain.DTO_s.Responses;
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

        #region PropertyRegion
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
                                            CityId = Guid.Parse(propertyData.City),
                                            StateId = Guid.Parse(propertyData.State),
                                            CountryId = Guid.Parse(propertyData.Country),
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
                var properties = await _unitOfWorkService.Properties.GetAllAsync(x => x.OwnerId == userId && x.DeleteStatus == false);
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
        public async Task<DeletePropertyResponse> DeleteProperties(DeletePropertyRequest request, string UserID)
        {
            try
            {
                DeletePropertyResponse response = new DeletePropertyResponse();

                // Validate UserID
                if (string.IsNullOrEmpty(UserID) || !Guid.TryParse(UserID, out var userId))
                {
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Fetch user to validate role
                var userData = await _unitOfWorkService.Users.GetByIdAsync(userId);
                if (userData == null || userData.Role != (int)TypeOfUserEnum.Owner)
                {
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Validate property IDs
                if (request.PropertyIDs == null || !request.PropertyIDs.Any())
                {
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Fetch properties belonging to the user that match the IDs
                var propertyIds = request.PropertyIDs.Select(id => Guid.Parse(id)).ToList();
                var properties = await _unitOfWorkService.Properties.GetAllAsync(x => x.OwnerId == userId && propertyIds.Contains(x.PropertyId) && !x.DeleteStatus);

                if (properties == null || !properties.Any())
                {
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Collect associated address IDs
                var addressIds = properties.Select(p => p.AddressId).Distinct().ToList();

                // Mark properties and their addresses as deleted
                foreach (var property in properties)
                {
                    property.DeleteStatus = true;
                    property.UpdatedAt = DateTime.Now;
                }

                var addresses = await _unitOfWorkService.Addresses.GetAllAsync(a => addressIds.Contains(a.AddressId) && !a.DeleteStatus);
                foreach (var address in addresses)
                {
                    address.DeleteStatus = true;
                    address.UpdatedAt = DateTime.Now;
                }

                // Save changes
                await _unitOfWorkService.Properties.BulkUpdate(properties.ToList());
                await _unitOfWorkService.Addresses.BulkUpdate(addresses.ToList());

                if (await _unitOfWorkService.Properties.SaveChangesAsync() > 0)
                {
                    response.UserID = UserID;
                    response.DeletedPropertyIDs = properties.Select(p => p.PropertyId.ToString()).ToList();
                    response.StatusMessage = Constants.MSG_SUCCESS;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.StatusMessage = Constants.MSG_FAILED;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                }

                return response;
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                throw;
            }
        }

        #endregion


        #region RoomsRegion
        public async Task<CreateRoomsResponse> CreateRooms(CreateRoomsRequest request, string UserID)
        {
            var response = new CreateRoomsResponse();

            try
            {
                // Validate UserID
                if (string.IsNullOrEmpty(UserID) || !Guid.TryParse(UserID, out var userGuid))
                {
                    response.StatusMessage = Constants.MSG_INVALID_USER_ID;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Check if the user exists and has the Owner role
                var user = await _unitOfWorkService.Users.GetByIdAsync(userGuid);
                if (user == null || user.Role != (int)TypeOfUserEnum.Owner)
                {
                    response.StatusMessage = Constants.MSG_UNAUTHORIZED;
                    response.StatusCode = StatusCodes.Status403Forbidden;
                    return response;
                }

                // Validate PropertyId
                if (string.IsNullOrEmpty(request.PropertyId) || !Guid.TryParse(request.PropertyId, out var propertyGuid))
                {
                    response.StatusMessage = Constants.MSG_INVALID_PROPERTY_ID;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                var property = await _unitOfWorkService.Properties.GetByIdAsync(propertyGuid);
                if (property == null || property.OwnerId != userGuid)
                {
                    response.StatusMessage = Constants.MSG_PROPERTY_NOT_FOUND_OR_UNAUTHORIZED;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Create Room Entities
                if (request.RoomDetails == null || !request.RoomDetails.Any())
                {
                    response.StatusMessage = Constants.MSG_NO_ROOM_DETAILS;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                var rooms = new List<Room>();
                foreach (var roomDetail in request.RoomDetails)
                {
                    if (roomDetail != null)
                    {
                        rooms.Add(new Room
                        {
                            PropertyId = propertyGuid,
                            RoomNumber = roomDetail.RoomNumber,
                            MaxOccupancy = roomDetail.MaxOccupancy,
                            RentPerMonth = roomDetail.RentPerMonth,
                            OccupiedStatus = roomDetail.OccupiedStatus,
                        });
                        response.CreatedRoomNumbers.Add(roomDetail.RoomNumber);
                    }
                }

                // Save Rooms in Bulk
                await _unitOfWorkService.Rooms.BulkSave(rooms);

               
                    response.UserID = UserID;
                    response.StatusMessage = Constants.MSG_SUCCESS;
                    response.StatusCode = StatusCodes.Status200OK;
                
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                response.StatusMessage = Constants.MSG_EXCEPTION;
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }

        public async Task<UpdateRoomsResponse> UpdateRooms(UpdateRoomsRequest request, string UserID)
        {
            var response = new UpdateRoomsResponse();

            try
            {
                // Validate UserID
                if (string.IsNullOrEmpty(UserID) || !Guid.TryParse(UserID, out var userGuid))
                {
                    response.StatusMessage = Constants.MSG_INVALID_USER_ID;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Check if the user exists and has the Owner role
                var user = await _unitOfWorkService.Users.GetByIdAsync(userGuid);
                if (user == null || user.Role != (int)TypeOfUserEnum.Owner)
                {
                    response.StatusMessage = Constants.MSG_UNAUTHORIZED;
                    response.StatusCode = StatusCodes.Status403Forbidden;
                    return response;
                }

                // Validate PropertyId
                if (string.IsNullOrEmpty(request.PropertyId) || !Guid.TryParse(request.PropertyId, out var propertyGuid))
                {
                    response.StatusMessage = Constants.MSG_INVALID_PROPERTY_ID;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Check if property exists and is owned by the user
                var property = await _unitOfWorkService.Properties.GetByIdAsync(propertyGuid);
                if (property == null || property.OwnerId != userGuid)
                {
                    response.StatusMessage = Constants.MSG_PROPERTY_NOT_FOUND_OR_UNAUTHORIZED;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Validate RoomDetails
                if (request.RoomDetails == null || !request.RoomDetails.Any())
                {
                    response.StatusMessage = Constants.MSG_NO_ROOM_DETAILS;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                foreach (var roomDetail in request.RoomDetails)
                {
                    if (roomDetail != null && Guid.TryParse(roomDetail.RoomId, out var roomGuid))
                    {
                        // Fetch the room and validate its existence
                        var room = await _unitOfWorkService.Rooms.GetByIdAsync(roomGuid);
                        if (room != null && room.PropertyId == propertyGuid)
                        {
                            // Update room details
                            room.RoomNumber = roomDetail.RoomNumber ?? room.RoomNumber;
                            room.MaxOccupancy = roomDetail.MaxOccupancy;
                            room.RentPerMonth = roomDetail.RentPerMonth;
                            room.OccupiedStatus = roomDetail.OccupiedStatus;

                            // Add updated room ID to the response
                            response.UpdatedRoomIds.Add(roomDetail.RoomId);
                        }
                        else
                        {
                            response.StatusMessage = Constants.MSG_NO_ROOMS_FOUND;
                            response.StatusCode = StatusCodes.Status404NotFound;
                        }
                    }
                }

                // Save changes to the database
                if (await _unitOfWorkService.Rooms.SaveChangesAsync() > 0)
                {
                    response.UserID = UserID;
                    response.StatusMessage = Constants.MSG_SUCCESS;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.StatusMessage = Constants.MSG_FAILED;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                response.StatusMessage = Constants.MSG_EXCEPTION;
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }


        public async Task<GetAllRoomsResponse> GetAllRoomsByPropertyId(string propertyId, string userId)
        {
            var response = new GetAllRoomsResponse();

            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(propertyId) || string.IsNullOrEmpty(userId))
                {
                    response.StatusMessage = Constants.MSG_INVALID_REQUEST;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Validate PropertyId and UserID formats
                if (!Guid.TryParse(propertyId, out var propertyGuid) || !Guid.TryParse(userId, out var userGuid))
                {
                    response.StatusMessage = Constants.MSG_INVALID_REQUEST;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Verify the user exists and has the Owner role
                var user = await _unitOfWorkService.Users.GetByIdAsync(userGuid);
                if (user == null || user.Role != (int)TypeOfUserEnum.Owner)
                {
                    response.StatusMessage = Constants.MSG_UNAUTHORIZED;
                    response.StatusCode = StatusCodes.Status403Forbidden;
                    return response;
                }

                // Fetch the property and validate ownership
                var property = await _unitOfWorkService.Properties.GetByIdAsync(propertyGuid);
                if (property == null || property.OwnerId != userGuid)
                {
                    response.StatusMessage = Constants.MSG_PROPERTY_NOT_FOUND_OR_UNAUTHORIZED;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Fetch rooms for the property
                var rooms = await _unitOfWorkService.Rooms.GetAllAsync(r => r.PropertyId == propertyGuid && r.DeleteStatus == false);

                if (rooms != null && rooms.Any())
                {
                    response.RoomDetails = rooms.Select(r => new StayEasePro_Domain.DTO_s.Responses.RoomDetails
                    {
                        RoomId = r.RoomId.ToString().ToUpper(),
                        RoomNumber = r.RoomNumber,
                        MaxOccupancy = r.MaxOccupancy,
                        RentPerMonth = r.RentPerMonth,
                        OccupiedStatus = r.OccupiedStatus
                    }).ToList();

                    response.StatusMessage = Constants.MSG_SUCCESS;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.StatusMessage = Constants.MSG_NO_ROOMS_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                }
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                response.StatusMessage = Constants.MSG_EXCEPTION;
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }


        public async Task<DeleteRoomsResponse> DeleteRooms(DeleteRoomsRequest request, string userId)
        {
            var response = new DeleteRoomsResponse();

            try
            {
                // Validate inputs
                if (request == null || request.RoomIds == null || !request.RoomIds.Any() || string.IsNullOrEmpty(userId))
                {
                    response.StatusMessage = Constants.MSG_INVALID_REQUEST;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Validate UserId format
                if (!Guid.TryParse(userId, out var userGuid))
                {
                    response.StatusMessage = Constants.MSG_INVALID_REQUEST;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Verify user exists and has Owner role
                var user = await _unitOfWorkService.Users.GetByIdAsync(userGuid);
                if (user == null || user.Role != (int)TypeOfUserEnum.Owner)
                {
                    response.StatusMessage = Constants.MSG_UNAUTHORIZED;
                    response.StatusCode = StatusCodes.Status403Forbidden;
                    return response;
                }

                // Fetch rooms based on RoomIds and validate ownership
                var roomGuids = request.RoomIds.Select(Guid.Parse).ToList();
                var rooms = await _unitOfWorkService.Rooms.GetAllAsync(r => roomGuids.Contains(r.RoomId));

                if (rooms == null || !rooms.Any())
                {
                    response.StatusMessage = Constants.MSG_NO_ROOMS_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Verify all rooms belong to properties owned by the user
                foreach (var room in rooms)
                {
                    var property = await _unitOfWorkService.Properties.GetByIdAsync(room.PropertyId);
                    if (property == null || property.OwnerId != userGuid)
                    {
                        response.StatusMessage = Constants.MSG_UNAUTHORIZED_DELETE;
                        response.StatusCode = StatusCodes.Status403Forbidden;
                        return response;
                    }
                }

                // Update DeleteStatus for rooms
                foreach (var room in rooms)
                {
                    room.DeleteStatus = true;
                    
                }

                await _unitOfWorkService.Rooms.BulkUpdate(rooms.ToList());

                if (await _unitOfWorkService.Rooms.SaveChangesAsync() > 0)
                {
                    response.StatusMessage = Constants.MSG_SUCCESS;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.DeletedRoomIds = rooms.Select(r => r.RoomId.ToString()).ToList();
                }
                else
                {
                    response.StatusMessage = Constants.MSG_FAILED;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                response.StatusMessage = Constants.MSG_EXCEPTION;
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }




        #endregion

    }
}
