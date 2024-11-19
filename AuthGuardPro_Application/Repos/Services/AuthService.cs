using AuthGuardPro_Application.DTO_s.Requests;
using AuthGuardPro_Application.Repos.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthGuardPro_Application.Repos.Services
{
    // This service implements the IJWTTokenGeneration interface, providing a method to generate JWT tokens
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration; // IConfiguration instance to access app settings

       // private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor that accepts IConfiguration to access JWT settings from app configuration
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
         
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
                new Claim(JwtRegisteredClaimNames.Sub, request.UserID),
                
                // A unique identifier (JTI) for each token, using a GUID for uniqueness
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                
                // Name claim representing the UserID, as an additional identifier
                new Claim(ClaimTypes.Name, request.UserID),
                
                // NameIdentifier claim to hold the username, adding it for more detailed user info in the token
                new Claim(ClaimTypes.NameIdentifier, request.UserName),
                
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

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
