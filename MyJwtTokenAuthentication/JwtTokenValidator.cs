using Microsoft.IdentityModel.Tokens;
using MyJwtTokenAuthentication.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyJwtTokenAuthentication
{
    public  class JwtTokenValidator: IJwtTokenValidator
    {
        private readonly IJwtSecretKeyProvider _secretKeyProvider;

        public JwtTokenValidator(IJwtSecretKeyProvider secretKeyProvider)
        {
            _secretKeyProvider = secretKeyProvider;
        }
        //private static readonly string SecretKey = "my-demo-key";// SecretKeyGenerator.GenerateSecretKey(); // The same key used to generate the token
        private const string Issuer = "your-app";                      // The same issuer used during generation
        private const string Audience = "your-audience";               // The same audience used during generation

       
        
        public  ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                // Step 1: Define the token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,                 // Validate the issuer
                    ValidIssuer = Issuer,                  // Expected issuer

                    ValidateAudience = true,               // Validate the audience
                    ValidAudience = Audience,              // Expected audience

                    ValidateLifetime = true,               // Ensure token has not expired
                    //ClockSkew = TimeSpan.Zero,             // Allow no time drift for expiration check

                    ValidateIssuerSigningKey = true,       // Ensure the token's signature is valid
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKeyProvider.GetSecretKey())) // Signing key
                };

                // Step 2: Create a token handler and validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

                // Step 3: Return the validated ClaimsPrincipal (contains claims from the token)
                return principal;
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null; // Token is invalid
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null; // Some other error occurred
            }
        }
    }
}
