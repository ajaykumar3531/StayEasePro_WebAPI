using AuthGuardPro_Application.DTO_s.DTO;
using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Application.Repos.Contracts;
using StayEasePro_Core.DbRepos.Services;
using StayEasePro_Core.Entities;
using StayEasePro_Domain.DTO_s.Requests;
using StayEasePro_Domain.DTO_s.Responses;

namespace AuthGuardPro_Application.Repos.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userContext;
        private readonly IAuthService _authService;
        private readonly ILoggerService _logger;
        private readonly IUnitOfWorkService _unitOfWorkService;
        public UserService(IBaseRepository<User> userContext, IAuthService authService, ILoggerService logger, IUnitOfWorkService unitOfWorkService)
        {
            _userContext = userContext;
            _authService = authService;
            _logger = logger;
            _unitOfWorkService = unitOfWorkService;
        }

        /// <summary>
        /// Creates a new user based on the provided request data.
        /// Checks if a user with the same username or email already exists.
        /// If not, creates a new user with a salted and hashed password, then saves it to the database.
        /// </summary>
        /// <param name="request">The user creation request containing username, email, and password.</param>
        /// <returns>A task that represents the asynchronous operation, with a response containing user details or an error status.</returns>
        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            try
            {
                CreateUserResponse response = new CreateUserResponse();

                // Check if the request is null and return a no content response if it is.
                if (request == null)
                {
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    return response;
                }

                var existedUser = (await _userContext.GetAllAsync()).FirstOrDefault(x =>
      x.DeleteStatus == false &&
      ((x.Email != null && !string.IsNullOrWhiteSpace(request.Email) && x.Email.ToLower() == request.Email.ToLower()) ||
       (x.Phone != null && !string.IsNullOrWhiteSpace(request.Phone) && x.Phone.ToLower() == request.Phone.ToLower())));

                if (existedUser != null)
                {
                    // If the user exists, return a response indicating that the user data was found.
                    response.StatusMessage = Constants.MSG_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status302Found;
                }
                else
                {
                    // Generate a salt and hash the password with the salt for secure storage.
                    string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password + salt);

                    // Create a new user object with the provided details and hashed password.
                    var userData = new User()
                    {
                        Email = request.Email,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        DeleteStatus = false,
                        Salt = salt,
                        PasswordHash = hashedPassword,
                        Phone = request.Phone,
                        Role = request.Role,
                        DateOfBirth = request.DOB,
                        JoinedDate = request.JoinedDate,
                        ExpectedJoinDate = request.ExpectedJoindDate
                    };

                    // Add the new user to the context and save changes.
                    await _userContext.AddAsync(userData);
                    if (await _userContext.SaveChangesAsync() > 0)
                    {
                        response.DateCreated = userData.CreatedAt;
                        response.Email = userData.Email;
                        response.UserId = userData.UserId.ToString().ToUpper();
                        response.StatusMessage = Constants.MSG_USER_ADD;
                        response.StatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        // If save fails, set response to no content status.
                        response.StatusCode = StatusCodes.Status204NoContent;
                        response.StatusMessage = Constants.MSG_REQ_NULL;
                    }
                }

                return response; // Return the populated response.
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);

                throw;
            }
        }

        /// <summary>
        /// Deletes a user by marking the user as deleted in the database based on the provided request data.
        /// Finds the user by matching username and email and updates the record to mark it as deleted if found.
        /// </summary>
        /// <param name="request">The delete request containing username and email.</param>
        /// <returns>A task representing the asynchronous operation, with a response indicating success or failure status.</returns>
        public async Task<DeleteResponse> DeleteUser(DeleteRequest request)
        {
            DeleteResponse response = new DeleteResponse();
            try
            {
                // Check if the request is null and return a no content response if it is.
                if (request == null)
                {
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    return response;
                }

                // Find an existing user by username and email who is not already marked as deleted.
                User existedUser = (await _userContext.GetAllAsync())?.ToList()?
                    .FirstOrDefault(x =>

                        x.Email.ToLower() == request.Email.ToLower() &&
                        x.DeleteStatus == false);

                if (existedUser != null)
                {
                    // Mark the user as deleted and update the timestamp.
                    existedUser.DeleteStatus = true;
                    existedUser.UpdatedAt = DateTime.Now;
                    await _userContext.UpdateAsync(existedUser);

                    // Save changes and check if the deletion was successful.
                    if (await _userContext.SaveChangesAsync() > 0)
                    {
                        // Populate response with success status and user details.

                        response.Email = request.Email;
                        response.StatusMessage = Constants.MSG_SUCCESS;
                        response.StatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        // If saving changes failed, return a not found status.

                        response.Email = request.Email;
                        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                        response.StatusCode = StatusCodes.Status404NotFound;
                    }
                }
                else
                {
                    // If no matching user is found, return a not found status.

                    response.Email = request.Email;
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                }

                return response; // Return the populated response.
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                // Log or handle the exception as needed. Re-throwing allows higher-level error handling.
                throw;
            }
        }

        /// <summary>
        /// Resets the password for an existing user based on the provided request data.
        /// Finds the user by matching username and email, generates a new salted and hashed password, and updates the record.
        /// </summary>
        /// <param name="request">The forgot password request containing username, email, and new password.</param>
        /// <returns>A task representing the asynchronous operation, with a response indicating success or failure status.</returns>
        public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request)
        {
            ForgotPasswordResponse response = new ForgotPasswordResponse();
            try
            {
                // Check if the request is null and return a no content response if it is.
                if (request == null)
                {
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                    return response;
                }

                // Find an existing user by username and email who is not already marked as deleted.
                User existedUser = (await _userContext.GetAllAsync())?.ToList()?
                    .FirstOrDefault(x =>

                        x.Email.ToLower() == request.Email.ToLower() &&
                        x.DeleteStatus == false);

                if (existedUser != null)
                {
                    // Generate a new salt and hash the new password with the salt.
                    string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password + salt);

                    // Update the user with the new password hash, salt, and timestamp.
                    existedUser.PasswordHash = hashedPassword;
                    existedUser.Salt = salt;
                    existedUser.UpdatedAt = DateTime.Now;
                    await _userContext.UpdateAsync(existedUser);

                    // Save changes and check if the password reset was successful.
                    if (await _userContext.SaveChangesAsync() > 0)
                    {
                        // Populate response with success status and user details.

                        response.Email = request.Email;
                        response.StatusMessage = Constants.MSG_SUCCESS;
                        response.StatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        // If saving changes failed, return a not found status.

                        response.Email = request.Email;
                        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                        response.StatusCode = StatusCodes.Status404NotFound;
                    }
                }
                else
                {
                    // If no matching user is found, return a not found status.

                    response.Email = request.Email;
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                }

                return response; // Return the populated response.
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                // Log or handle the exception as needed. Re-throwing allows higher-level error handling.
                throw;
            }
        }

        public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
        {
            LoginUserResponse response = new LoginUserResponse();
            try
            {
                if (request == null)
                {
                    response.StatusCode = StatusCodes.Status204NoContent;
                    response.StatusMessage = Constants.MSG_REQ_NULL;
                }
                else
                {
                    User existedUserData = (await _userContext.GetAllAsync())?
                                                .FirstOrDefault(x =>
                                                (x.Email.ToLower() == request.Email.ToLower() || x.Phone.ToLower() == request.Phone.ToLower())
                                                && x.DeleteStatus == false);

                    if (existedUserData != null)
                    {
                        string salt = existedUserData.Salt;
                        bool verifyPassword = BCrypt.Net.BCrypt.Verify(request.Password + salt, existedUserData.PasswordHash);

                        if (verifyPassword)
                        {
                            TokenRequest tokenRequest = new TokenRequest()
                            {
                                Email = existedUserData.Email,
                                UserID = existedUserData.UserId.ToString(),

                            };


                            response.JWTToken = await _authService.TokenGeneration(tokenRequest);

                            response.Email = request.Email;
                            response.StatusMessage = Constants.MSG_LOGIN_SUCC;
                            response.StatusCode = StatusCodes.Status200OK;

                        }
                        else
                        {

                            response.Email = request.Email;
                            response.StatusCode = StatusCodes.Status400BadRequest;
                            response.StatusMessage = Constants.MSG_LOGIN_FAIL;
                        }
                    }
                    else
                    {

                        response.Email = request.Email;
                        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                        response.StatusCode = StatusCodes.Status404NotFound;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                await _logger.LocalLogs(ex);
                throw ex;
            }
        }

        public async Task<JoinDetailsResponse> JoinUser(JoinDetailsRequest request)
        {
            var response = new JoinDetailsResponse();

            try
            {
                // Validate the request
                if (request == null || string.IsNullOrEmpty(request.UserId) || request.AddressDetails == null || string.IsNullOrEmpty(request.RoomId))
                {
                    response.StatusMessage = Constants.MSG_INVALID_REQUEST;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Parse UserId and RoomId
                if (!Guid.TryParse(request.UserId, out var userGuid) || !Guid.TryParse(request.RoomId, out var roomGuid))
                {
                    response.StatusMessage = Constants.MSG_INVALID_REQUEST;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Fetch the user from the database
                var user = await _unitOfWorkService.Users.GetByIdAsync(userGuid);
                if (user == null)
                {
                    response.StatusMessage = Constants.MSG_USER_NOT_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                // Add the address data to the Address table
                var address = new Address
                {
                    Street = request.AddressDetails.Street,
                    City = request.AddressDetails.City,
                    State = request.AddressDetails.State,
                    Country = request.AddressDetails.Country,
                    ZipCode = request.AddressDetails.ZipCode,
                    DeleteStatus = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _unitOfWorkService.Addresses.AddAsync(address);

                if (await _unitOfWorkService.Addresses.SaveChangesAsync() <= 0)
                {
                    response.StatusMessage = Constants.MSG_ADDRESS_SAVE_FAILED;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    return response;
                }

                // Update the user's AddressId
                user.AddressId = address.AddressId;
                user.PropertyId = Guid.Parse(request.PropertyID);
                await _unitOfWorkService.Users.UpdateAsync(user);

                if (await _unitOfWorkService.Users.SaveChangesAsync() <= 0)
                {
                    response.StatusMessage = Constants.MSG_USER_UPDATE_FAILED;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    return response;
                }

                // Create a tenant record
                var tenant = new Tenant
                {
                    TenantId = Guid.NewGuid(),
                    UserId = userGuid,
                    RoomId = roomGuid,
                    CheckInDate = DateTime.Now,
                    RentDueDate = DateTime.Now.AddMonths(1), // Assuming rent is due in 1 month
                    ActiveStatus = true,

                };
                await _unitOfWorkService.Tenants.AddAsync(tenant);

                if (await _unitOfWorkService.Tenants.SaveChangesAsync() > 0)
                {
                    response.StatusMessage = Constants.MSG_SUCCESS;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.UserId = request.UserId;
                    response.AddressId = address.AddressId.ToString();
                    response.TenantId = tenant.TenantId.ToString();
                }
                else
                {
                    response.StatusMessage = Constants.MSG_TENANT_SAVE_FAILED;
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

    }
}
