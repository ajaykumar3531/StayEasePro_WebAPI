using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.Repos.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StayEasePro_Domain.DTO_s.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthGuardPro_Application.Repos.Services
{
    // This service implements the IJWTTokenGeneration interface, providing a method to generate JWT tokens
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration; // IConfiguration instance to access app settings
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _loggerService; 

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor,ILoggerService loggerService)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _loggerService = loggerService;
        }


        // Asynchronous method to generate a JWT token based on the provided TokenRequest
        public async Task<string> TokenGeneration(TokenRequest request)
        {
            // Retrieve the "JwtSettings" section from the configuration (e.g., appsettings.json)
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Create a symmetric security key using the "Secret" key from JwtSettings, which is essential for token signing
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));

            // Define signing credentials for the token, using HMAC-SHA256 algorithm for signing
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims for the JWT payload, which will hold user-specific data
            var claims = new[]
            {
                // A standard claim for the subject of the token, here using the UserID from the request
                new Claim(JwtRegisteredClaimNames.Name, request.UserID),
                
                // A unique identifier (JTI) for each token, using a GUID for uniqueness
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               
                // Standard email claim in the JWT, using the Email from the TokenRequest
                new Claim(JwtRegisteredClaimNames.Email, request.Email)
            };

            // Instantiate the JWT token with the defined parameters, including issuer, audience, claims, expiry, and signing credentials
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"], // Set issuer from configuration
                audience: jwtSettings["Audience"], // Set audience from configuration
                claims: claims, // Attach user claims
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])), // Set token expiration based on configured time
                signingCredentials: credentials // Set signing credentials using the HMAC-SHA256 key
            );

            
            // Serialize the token into a JWT string and return it to the caller
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task AuthorizeUser()
        {
            try
            {
                // Access the current HttpContext
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext?.User?.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
                {
                    // Retrieve all claims from the authenticated user's context
                    var claims = identity.Claims;

                    var userID = claims.FirstOrDefault(c => c.Type == "name")?.Value;
                    var email = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;


                    // Store or use claims globally, e.g., via static properties or dependency injection
                    GlobalUserContext.UserID = userID.ToUpper().ToString();
                    GlobalUserContext.Email = email;

                }
                else
                {
                    
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }
            }
            catch (Exception ex)
            {
                await _loggerService.LocalLogs(ex);
                throw;
            }
        }

    }
}
