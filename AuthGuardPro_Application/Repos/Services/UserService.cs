using AuthGuardPro_Application.DTO_s.DTO;
using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.DTO_s.Responses;
using AuthGuardPro_Application.Repos.Contracts;
using AuthGuardPro_Infrastucture.Repository.Contracts;
using StayEasePro_Core.Entities;

namespace AuthGuardPro_Application.Repos.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userContext;
        private readonly IAuthService _authService;
        private readonly ILoggerService _logger;
        public UserService(IBaseRepository<User> userContext, IAuthService authService, ILoggerService logger)
        {
            _userContext = userContext;
            _authService = authService;
            _logger = logger;
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

                //// Check if a user with the same username or email already exists and is not marked as deleted.
                //var existedUser = (await _userContext.GetAllAsync())?.ToList()?
                //    .FirstOrDefault(x =>
                //        (x.Email.ToLower() == request.Username.ToLower() || x.Email.ToLower() == request.Email.ToLower()) && x.IsDeleted == false);

                //if (existedUser != null)
                //{
                //    // If the user exists, return a response indicating that the user data was found.
                //    response.StatusMessage = Constants.MSG_DATA_FOUND;
                //    response.StatusCode = StatusCodes.Status302Found;
                //}
                else
                {
                    // Generate a salt and hash the password with the salt for secure storage.
                    string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password + salt);

                    // Create a new user object with the provided details and hashed password.
                    var userData = new User()
                    {
                        //Username = request.Username,
                        //Email = request.Email,
                        //IsDeleted = false,
                        //PasswordHash = hashedPassword,
                        //Salt = salt,
                    };

                    // Add the new user to the context and save changes.
                    await _userContext.AddAsync(userData);
                    if (await _userContext.SaveChangesAsync() > 0)
                    {
                      
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
                _logger.LocalLogs(ex);
                // Log the exception or handle it as needed. Re-throwing it can help with higher-level error handling.
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

                //// Find an existing user by username and email who is not already marked as deleted.
                //User existedUser = (await _userContext.GetAllAsync())?.ToList()?
                //    .FirstOrDefault(x =>
                //        x.Username.ToLower() == request.Username.ToLower() &&
                //        x.Email.ToLower() == request.Email.ToLower() &&
                //        x.IsDeleted == false);

                //if (existedUser != null)
                //{
                //    // Mark the user as deleted and update the timestamp.
                //    existedUser.IsDeleted = true;
                //    existedUser.DateUpdated = DateTime.Now;
                //    await _userContext.UpdateAsync(existedUser);

                //    // Save changes and check if the deletion was successful.
                //    if (await _userContext.SaveChangesAsync() > 0)
                //    {
                //        // Populate response with success status and user details.
                //        response.Username = request.Username;
                //        response.Email = request.Email;
                //        response.StatusMessage = Constants.MSG_SUCCESS;
                //        response.StatusCode = StatusCodes.Status200OK;
                //    }
                //    else
                //    {
                //        // If saving changes failed, return a not found status.
                //        response.Username = request.Username;
                //        response.Email = request.Email;
                //        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                //        response.StatusCode = StatusCodes.Status404NotFound;
                //    }
                //}
                else
                {
                    // If no matching user is found, return a not found status.
                    response.Username = request.Username;
                    response.Email = request.Email;
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                }

                return response; // Return the populated response.
            }
            catch (Exception ex)
            {
                _logger.LocalLogs(ex);
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

                //// Find an existing user by username and email who is not already marked as deleted.
                //User existedUser = (await _userContext.GetAllAsync())?.ToList()?
                //    .FirstOrDefault(x =>
                //        x.Username.ToLower() == request.Username.ToLower() &&
                //        x.Email.ToLower() == request.Email.ToLower() &&
                //        x.IsDeleted == false);

                //if (existedUser != null)
                //{
                //    // Generate a new salt and hash the new password with the salt.
                //    string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password + salt);

                //    // Update the user with the new password hash, salt, and timestamp.
                //    existedUser.PasswordHash = hashedPassword;
                //    existedUser.Salt = salt;
                //    existedUser.DateUpdated = DateTime.Now;
                //    await _userContext.UpdateAsync(existedUser);

                //    // Save changes and check if the password reset was successful.
                //    if (await _userContext.SaveChangesAsync() > 0)
                //    {
                //        // Populate response with success status and user details.
                //        response.Username = request.Username;
                //        response.Email = request.Email;
                //        response.StatusMessage = Constants.MSG_SUCCESS;
                //        response.StatusCode = StatusCodes.Status200OK;
                //    }
                //    else
                //    {
                //        // If saving changes failed, return a not found status.
                //        response.Username = request.Username;
                //        response.Email = request.Email;
                //        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                //        response.StatusCode = StatusCodes.Status404NotFound;
                //    }
                //}
                //else
                {
                    // If no matching user is found, return a not found status.
                    response.Username = request.Username;
                    response.Email = request.Email;
                    response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                    response.StatusCode = StatusCodes.Status404NotFound;
                }

                return response; // Return the populated response.
            }
            catch (Exception ex)
            {
                _logger.LocalLogs(ex);
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
                //else
                //{
                //    User existedUserData = (await _userContext.GetAllAsync())?.FirstOrDefault(x => x.Username.ToLower() == request.Username.ToLower() && x.Email.ToLower() == request.Email && x.IsDeleted == false);

                //    if (existedUserData != null)
                //    {
                //        string salt = existedUserData.Salt;
                //        bool verifyPassword = BCrypt.Net.BCrypt.Verify(request.Password + salt, existedUserData.PasswordHash);

                //        if (verifyPassword)
                //        {
                //            TokenRequest tokenRequest = new TokenRequest()
                //            {
                //                Email = existedUserData.Email,
                //                UserID = existedUserData.UserId.ToString(),
                //                UserName = existedUserData.Username 
                //            };


                //            response.JWTToken = await _authService.TokenGeneration(tokenRequest);
                //            response.Username = request.Username;
                //            response.Email = request.Email;
                //            response.StatusMessage = Constants.MSG_LOGIN_SUCC;
                //            response.StatusCode = StatusCodes.Status200OK;
                //        }
                //        else
                //        {
                //            response.Username = request.Username;
                //            response.Email = request.Email;
                //            response.StatusCode = StatusCodes.Status400BadRequest;
                //            response.StatusMessage = Constants.MSG_LOGIN_FAIL;
                //        }
                //    }
                //    else
                //    {
                //        response.Username = request.Username;
                //        response.Email = request.Email;
                //        response.StatusMessage = Constants.MSG_NO_DATA_FOUND;
                //        response.StatusCode = StatusCodes.Status404NotFound;
                //    }
                //}
                return response;
            }
            catch (Exception ex)
            {
                _logger.LocalLogs(ex);
                throw ex;
            }
        }
    }
}
