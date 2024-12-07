using MyJwtTokenAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using MyJwtTokenAuthentication.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyJwtTokenAuthentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IJwtSecretKeyProvider _secretKeyProvider;
        public JwtTokenGenerator(IJwtSecretKeyProvider secretKeyProvider)
        {
            _secretKeyProvider = secretKeyProvider;
        }
        private string SecretKey = "my-demo-key";//SecretKeyGenerator.GenerateSecretKey();//"your-very-strong-secret-key"; // Replace with a secure key
        private const string Issuer = "your-app";                      // Replace with your application name
        private const string Audience = "your-audience";               // Replace with your intended audience


        #region tokengenerationcode

        //public string IJwtTokenGenerator.GenerateToken(string username, string role, int expirationMinutes = 30)
        //{
        //   return JwtTokenGenerator.GenerateToken( username,  role,  expirationMinutes );
        //}
        public string GenerateToken(string username, string role, int expirationMinutes = 30)
        {
            return GenerateStaticToken(username, role, _secretKeyProvider.GetSecretKey(), expirationMinutes);
        }
        private static string GenerateStaticToken(string username, string role, string secretKey, int expirationMinutes = 30)
        {
            // Step 1: Define claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),              // Subject (username)
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token identifier
                new Claim(ClaimTypes.Role, role),                             // User's role
                //new Claim("customClaim", "customValue")                       // Any custom claims
            };

            // Step 2: Generate signing key
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));//if SecretKey is not made static,then compile error:object reference required for non-static field SecretKey
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Step 3: Define token properties
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes), // Token expiration
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = credentials
            };

            // Step 4: Create the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            // Step 5: Write the token as a string
            return tokenHandler.WriteToken(securityToken);
        }

        #endregion

    }

}
